using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCApi.IRepo
{
    public interface IGenericRepo<T> where T : class
    {
        List<T> GetAll();
        T GetByID(int id);
        int Create(T item);
        int Update(T item);
    }
}
