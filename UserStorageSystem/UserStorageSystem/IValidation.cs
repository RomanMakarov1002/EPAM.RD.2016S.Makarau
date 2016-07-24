using System;


namespace UserStorageSystem
{
    public interface IValidation<T>
    {
        bool IsValid();
    }
}
