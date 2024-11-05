using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Negocio.Modelos;
using Negocio;
using System.Reflection;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Google.Protobuf.WellKnownTypes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class API : ControllerBase
    {
       
        ProductosAPI prodApi=new ProductosAPI();
        //Datos product;
        private readonly ILogger<API> _logger;

        public API(ILogger<API> logger)
        {
            _logger = logger;
        }

        [HttpGet("products")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult GetProducto()
        {
            try
            {
                _logger.LogInformation("SE MUESTRAN TODOS LOS PRODUCTOS");
                List<Datos> lisdatos = prodApi.GetProduct();
                return Ok(lisdatos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los productos.");
                return StatusCode(500, $"Error en el servidor al obtener los productos: {ex.Message}");
            }
        }
        [HttpGet("Category")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult GetCategories()
        {
            _logger.LogInformation("Iniciando la obtención de todas las categorías.");
            try
            {
                List<string> lisdatos = prodApi.GetCategories();
                return Ok(lisdatos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las categorías.");
                return StatusCode(500, $"Error en el servidor al obtener las categorías: {ex.Message}");
            }
        }


        [HttpGet("products/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult IdGet(int id)
        {
            _logger.LogInformation("Iniciando la búsqueda del producto con ID: {Id}", id);
            try
            {
                var producto =prodApi.GetId(id);

                if (producto == null)
                {
                    _logger.LogWarning("No se encontró el producto con ID: {Id}", id);
                    return NotFound();
                }
                return Ok(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el producto con ID: {Id}", id);
                return StatusCode(500, $"Error en el servidor al filtrar por Id:{ex.Message}");
            }
        }

        [HttpGet("products/category/{category}")] 
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCat(string category)
        {
            _logger.LogInformation("Iniciando la búsqueda de productos en la categoría: {Category}", category);
            try
            {
                var categoria = prodApi.GetCategory(category);

                if (categoria == null)
                {
                    _logger.LogWarning("No se encontraron productos en la categoría: {Category}", category);
                    return NotFound();
                }

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos en la categoría: {Category}", category);
                return StatusCode(500, $"Error en el servidor al filtrar por Categoría:{ex.Message}");
            }
        }


        [HttpPost("products")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult DatoPost([FromBody] Datos datosPost)
        {
            _logger.LogInformation("Iniciando la creación de un nuevo producto.");

            try
            {
                if (datosPost == null)
                {
                    _logger.LogWarning("El objeto de datos recibido es nulo.");
                    return BadRequest(datosPost);
                }
                if (datosPost.Id > 0)
                {
                    _logger.LogWarning("El ID del producto debe ser 0 al crear un nuevo producto. ID recibido: {Id}", datosPost.Id);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                Datos datoNuevo = prodApi.PostDat(datosPost);
                return Ok(datosPost);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar el nuevo producto.");
                return StatusCode(500, $"Error en el servidor al guardar el nuevo producto:{ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult PutDatos(int id, [FromBody] Datos datos)
        {
            _logger.LogInformation("Iniciando la actualización del producto con ID: {Id}", id);

            if (id == 0 || datos == null || datos.Id != id)
            {
                _logger.LogError("Error en la validación de los datos para actualizar el producto. ID ingresado: {Id}, ID en el cuerpo: {BodyId}", id, datos?.Id);
                return BadRequest();
            }

            try
            {
                _logger.LogInformation("Producto con ID: {Id} actualizado exitosamente.", id);
                prodApi.PutId(datos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el producto con ID: {Id}", id);
                return StatusCode(500, $"Error en el servidor al actualizar por Id:{ex.Message}");
            }

            return NoContent();
        }
        [HttpPut("products/{category}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult PutCat(string category, [FromBody] Datos datos)
        {
            _logger.LogInformation("Iniciando la actualización de productos en la categoría: {Category}", category);
            if (datos==null)
            {
                _logger.LogWarning("Los datos para actualizar en la categoría {Category} son nulos.", category);
                return BadRequest();
            }

            try
            {
                _logger.LogInformation("Productos en la categoría {Category} actualizados exitosamente.", category);
                prodApi.PutCategory(datos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar productos en la categoría: {Category}", category);
                return StatusCode(500, $"Error en el servidor al actualizar por Categoría:{ex.Message}");
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteDatos(int id)
        {
            _logger.LogInformation("Iniciando la eliminación del producto con ID: {Id}", id);
            if (id == 0)
            {
                _logger.LogWarning("El ID proporcionado para eliminar es inválido: {Id}", id);
                return BadRequest();
            }

            try
            {
                _logger.LogInformation("Producto con ID: {Id} eliminado exitosamente.", id);
                prodApi.DeleteID(id);
            }
            catch
            {
                _logger.LogError( "Error al eliminar el producto con ID: {Id}", id);
                return NotFound();
            }

            return NoContent();
        }
        [HttpDelete("products/{category}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCat(string category)
        {
            _logger.LogInformation("Iniciando la eliminación de la categoría: {Category}", category);
            if (category == null)
            {
                _logger.LogWarning("La categoría proporcionada para eliminar es nula o vacía.");
                return BadRequest();
            }

            try
            {
                _logger.LogInformation("Categoría: {Category} eliminada exitosamente.", category);
                prodApi.DeleteCategory(category);
            }
            catch
            {
                _logger.LogError("Error al eliminar la categoría: {Category}", category);
                return NotFound();
            }

            return NoContent();
        }


    }
}
