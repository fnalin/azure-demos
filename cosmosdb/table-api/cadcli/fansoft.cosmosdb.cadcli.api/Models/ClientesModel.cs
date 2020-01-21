using System.ComponentModel.DataAnnotations;

namespace fansoft.cosmosdb.cadcli.api.Models
{
    public class Cliente
    {
        [Required(ErrorMessage = "campo obrigatório")]
        public string Nome { get; set; }

        public Local Local { get; set; }

    }

    public class Local
    {
        [Required(ErrorMessage = "campo obrigatório")]
        public string Endereco { get; set; }
        [Required(ErrorMessage = "campo obrigatório")]
        public string Bairro { get; set; }
        [Required(ErrorMessage = "campo obrigatório")]
        public string Cidade { get; set; }
        [Required(ErrorMessage = "campo obrigatório")]
        public string Estado { get; set; }
        [Required(ErrorMessage = "campo obrigatório")]
        public string Pais { get; set; }
    }

    public static class ModelExtensions 
    {
        public static core.Cliente ToData(this Cliente vm)
        {
            return new core.Cliente {
                Nome = vm.Nome,
                Local = new core.Local {
                    Endereco = vm.Local.Endereco,
                    Bairro = vm.Local.Bairro,
                    Cidade = vm.Local.Cidade,
                    Estado = vm.Local.Estado,
                    Pais = vm.Local.Pais
                }
            };
        }
    } 
}
