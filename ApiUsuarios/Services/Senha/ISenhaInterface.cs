using ApiUsuarios.Models;

namespace ApiUsuarios.Services.Senha
{
    public interface ISenhaInterface 
    {
        void CriarSenhaHash(string senha, out byte[] senhaHash, out byte[] senhaSalt);
        bool VerificarSenhaHash(string senha, byte[] senhaHash, byte[] senhaSalt);
        string CriarToken(UsuarioModel usuario);
    }
}
