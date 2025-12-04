using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace WpfApp {
  public partial class MainWindow : Window {
    private readonly HttpClient _http = new(){ BaseAddress = new Uri("http://localhost:5000/") };
    public ObservableCollection<ProductView> Products { get; set; } = new();

    public MainWindow() {
      InitializeComponent();
      ProductsGrid.ItemsSource = Products;
      Loaded += async (_,__) => await Load();
    }

    private async System.Threading.Tasks.Task Load(string filter="") {
      try {
        var url = "api/products";
        var list = await _http.GetFromJsonAsync<ProductDto[]>(url);
        Products.Clear();
        foreach(var p in list) {
          if(string.IsNullOrWhiteSpace(filter) || p.Name.Contains(filter, StringComparison.OrdinalIgnoreCase))
            Products.Add(new ProductView{ Id=p.Id, Name=p.Name, Price=p.Price, CategoryName = p.Category?.Name ?? "(none)"});
        }
      } catch(Exception ex) {
        MessageBox.Show("Erro ao carregar produtos: " + ex.Message);
      }
    }

    private async void OnRefresh(object s, RoutedEventArgs e) => await Load();

    private async void OnFilter(object s, RoutedEventArgs e) => await Load(FilterText.Text);

    private async void OnNew(object s, RoutedEventArgs e) {
      var win = new EditWindow();
      if(win.ShowDialog()==true) {
        try {
          var dto = new ProductDto{ Name=win.NameText, Price=win.Price, CategoryId=win.CategoryId };
          var res = await _http.PostAsJsonAsync("api/products", dto);
          if(res.IsSuccessStatusCode) await Load();
          else MessageBox.Show($"Erro: {res.StatusCode}");
        } catch(Exception ex){ MessageBox.Show(ex.Message); }
      }
    }

    private async void OnEdit(object s, RoutedEventArgs e) {
      if(ProductsGrid.SelectedItem is not ProductView sel) { MessageBox.Show("Selecione um produto"); return; }
      try {
        var resp = await _http.GetFromJsonAsync<ProductDto>($"api/products/{sel.Id}");
        var win = new EditWindow{ NameText=resp.Name, Price=resp.Price, CategoryId=resp.CategoryId, ProductId=resp.Id };
        if(win.ShowDialog()==true) {
          var dto = new ProductDto{ Id=win.ProductId, Name=win.NameText, Price=win.Price, CategoryId=win.CategoryId };
          var req = new HttpRequestMessage(HttpMethod.Put,$"api/products/{dto.Id}"){ Content = JsonContent.Create(dto) };
          var res = await _http.SendAsync(req);
          if(res.IsSuccessStatusCode) await Load();
          else MessageBox.Show($"Erro: {res.StatusCode}");
        }
      } catch(Exception ex){ MessageBox.Show(ex.Message); }
    }

    private async void OnDelete(object s, RoutedEventArgs e) {
      if(ProductsGrid.SelectedItem is not ProductView sel) { MessageBox.Show("Selecione um produto"); return; }
      if(MessageBox.Show("Confirma excluir?","Confirm", MessageBoxButton.YesNo)!=MessageBoxResult.Yes) return;
      var res = await _http.DeleteAsync($"api/products/{sel.Id}");
      if(res.IsSuccessStatusCode) await Load();
      else MessageBox.Show($"Erro: {res.StatusCode}");
    }
  }

  public class ProductView { public int Id{get;set;} public string Name{get;set;} = ""; public decimal Price{get;set;} public string CategoryName{get;set;} = ""; }
  public class ProductDto { public int Id{get;set;} public string Name{get;set;} = ""; public decimal Price{get;set;} public int CategoryId{get;set;} public Category? Category{get;set;} }
  public class Category { public int Id{get;set;} public string Name{get;set;} = ""; }
}