using AutoEod3.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoEod3.Models.Result
{
    public enum TypeResults
    {
        CSV = 1,
        Excel = 2
    }


    public interface IResult
    {
        List<Header> Headers { get; set; }
        Encoding Cahrset { get; set; }
        void SaveRow(List<String> line);
        void SaveToFile(string path);

    }
}
