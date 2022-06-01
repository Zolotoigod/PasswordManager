using MenagerCore.DTO;

namespace MenagerCore
{
    public interface IKeyService
    {
        AESKeys InitKeys();
    }
}