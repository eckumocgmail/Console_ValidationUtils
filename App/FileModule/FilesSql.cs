using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FileResourcesSql
{
    /**
       * получение списка ресурсов  
       
   public string[] ResourceList( )
   {
       List<string> names = new List<string>();
       foreach(JObject next in WebApp.GetDataSource().Execute("select resource_name from resources"))
       {
           names.Add(next["resource_name"].Value<string>());
       }
       return names.ToArray();
   }

   /**
       * выгрузка бинарных данных из хранилище
       * /
   public static byte[] Download(string id)
   {
       return WebApp.GetDataSource().ReadBlob("select resource_data from resources where resource_id=" + id);
   }

   /**
       * загрузка бинарных данных в хранилище
       * /
   public static void Upload(string name, string mime, byte[] data)
   {
       WebApp.GetDataSource().InsertBlob("insert into resources (resource_name,mime_type,resource_data) values (\"" + name + "\",\"" + mime + "\",?)", "@bin_data", data);
   }*/

}