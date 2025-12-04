using System;
using System.Globalization;
using System.Windows;

namespace WpfApp {
  public partial class EditWindow : Window {
    public int ProductId { get; set; }
    public string NameText { get => NameBox.Text; set => NameBox.Text = value; }
    public decimal Price { get { decimal.TryParse(PriceBox.Text, NumberStyles.Currency, CultureInfo.InvariantCulture, out var v); return v; } set => PriceBox.Text = value.ToString(CultureInfo.InvariantCulture); }
    public int CategoryId { get { int.TryParse(CategoryBox.Text, out var v); return v; } set => CategoryBox.Text = value.ToString(); }

    public EditWindow() { InitializeComponent(); }

    private void OnSave(object s, RoutedEventArgs e) {
      if(string.IsNullOrWhiteSpace(NameText)) { MessageBox.Show("Nome é obrigatório"); return; }
      DialogResult = true;
    }
    private void OnCancel(object s, RoutedEventArgs e) => DialogResult = false;
  }
}