using System.ComponentModel.DataAnnotations;
namespace Api.Models {
  public class Product {
    public int Id {get;set;}
    [Required, StringLength(100)]
    public string Name {get;set;} = "";
    [Range(0,99999)]
    public decimal Price {get;set;}
    public int CategoryId {get;set;}
    public Category? Category {get;set;}
  }
}