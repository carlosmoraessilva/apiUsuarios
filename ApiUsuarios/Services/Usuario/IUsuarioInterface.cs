using ApiUsuarios.Dto.Usuario;
using ApiUsuarios.Models;

namespace ApiUsuarios.Services.Usuario
{
    public interface IUsuarioInterface
    {
        Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioCriacaoDto usuarioCriacaoDto);
    }
}
