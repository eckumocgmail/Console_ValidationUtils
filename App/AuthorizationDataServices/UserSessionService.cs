using ApplicationDb.Entities;

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


 
/// <summary>
/// Констроллер работающий с моделью сеанса
/// </summary>
/// <typeparam name="TModel"> тип модели сеанса </typeparam>
public abstract class UserSessionService<TModel> where TModel: class
{
    protected readonly APIAuthorization _authorization;
 


    /// <summary>
    /// Конструктор контроллера модели сеанса
    /// </summary>
    /// <param name="authorization"> сервис авторизации </param>
    public UserSessionService(APIAuthorization authorization)
    {
        _authorization = authorization;
    }

    /// <summary>
    /// Создание новоги экземпляра класса конструктором по-умолчанию
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public static T CreateWithDefaultConstructor<T>(Type type)
    {
        ConstructorInfo constructor = GetDefaultConstructor(type);
        return (T)constructor.Invoke(new object[0]);
    }


    /// <summary>
    /// Поиск конструктора класса по-умолчанию
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static ConstructorInfo GetDefaultConstructor(Type type)
    {

        ConstructorInfo contr = (from c in new List<ConstructorInfo>(type.GetConstructors()) where c.GetParameters().Length == 0 select c).FirstOrDefault();
        if (contr != null)
        {
            return contr;
        }
        else
        {
            foreach (ConstructorInfo constr in type.GetConstructors())
            {
                int pCOunt = constr.GetParameters().Count();
            }

            return null;
        }
    }

    /// <summary>
    /// Метод инициаллизации модели
    /// </summary>
    /// <param name="model"></param>
    public abstract void InitModel(TModel model);


    /// <summary>
    /// Создание новой модели сеанса
    /// </summary>
    /// <returns></returns>
    public TModel NewModel()
    {
        string key = GetType().FullName + "::" + typeof(TModel).FullName;
        ConcurrentDictionary<string, object> session = _authorization.Session();
        TModel order = CreateWithDefaultConstructor<TModel>(typeof(TModel));
        session[key] = order;
        InitModel(order);
        return (TModel)session[key];
    }


    /// <summary>
    /// Получение модели сеанса, если модели не существует выполняется её инициаллизация
    /// </summary>
    /// <returns></returns>
    public TModel GetModel()
    {
        string key = GetType().FullName + "::" + typeof(TModel).FullName;

        bool IsSigned = _authorization.IsSignin();
        if( IsSigned==false)
        {


            return null;
        }
        else
        {
            UserContext user = _authorization.Verify();
            ConcurrentDictionary<string, object> session = _authorization.Session();
            if (session == null)
            {
                _authorization.Signout();
                throw new Exception("Authentication failed");
            }
            else
            {
                if (session.ContainsKey(key) == false)
                {
                    TModel order =
                        CreateWithDefaultConstructor<TModel>(typeof(TModel));
                    session[key] = order;
                    InitModel(order);
                }
                return (TModel)session[key];

            }
        }
        
    }


    /// <summary>
    /// Получение модели сеанса из другого контроллера
    /// </summary>
    /// <typeparam name="T"> тип модели сеанса </typeparam>
    /// <param name="key"> ключ доступа </param>
    /// <returns></returns>
    public T GetAnotherModel<T>(string key)
    {
        ConcurrentDictionary<string, object> session = _authorization.Session();
        if (session.ContainsKey(key) == false)
        {
            T order =
                CreateWithDefaultConstructor<T>(typeof(T));
            session[key] = order;
        }
        return (T)session[key];
    }


    /// <summary>
    /// Получение модели сеанса из другого контроллера
    /// </summary>
    /// <param name="controllerType"> тип контроллера </param>
    /// <returns></returns>
    public TModel GetModel(Type controllerType)
    {
        string key = controllerType.FullName + "::" + typeof(TModel).FullName;
        ConcurrentDictionary<string, object> session = _authorization.Session();
        if (session == null)
        {
          
            throw new Exception("User not authorizated at system");
        }
        else
        {
            if (session.ContainsKey(key) == false)
            {
                TModel order =
                    CreateWithDefaultConstructor<TModel>(typeof(TModel));
                session[key] = order;
                InitModel(order);
            }
            return (TModel)session[key];
        }
    }
}
