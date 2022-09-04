using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CustomizationAttributes
{

    private static bool LogCustomization  = false;
    private static int CustomTypeAttributesCount = 0;

    /// <summary>
    /// Ключ -имя типа
    /// значение - карта 
    ///     имя атрибута-значение атрибута
    /// </summary>
    private static Dictionary<string, Dictionary<string, string>> CustomTypeAttriburtes =
        new Dictionary<string, Dictionary<string, string>>();

    /// <summary>
    /// Ключ -имя типа
    /// значение - карта 
    ///     имя свойства( метода ) - значение карта
    ///                 имя атрибута-значение атрибута
    /// </summary>
    private static int CustomMemberAttributesCount = 0;
    private static Dictionary<string, Dictionary<string, Dictionary<string, string>>> CustomMemberAttriburtes =
        new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();


    /// <summary>
    /// Добавление настраиваемого атрибута для типа
    /// </summary>
    /// <param name="name"></param>
    /// <param name="Value"></param>
    /// <returns></returns>
    public static int AddTypeAttr(Type type, string Name, string Value)
    {
        //проверяем наличие обьявления атрибута заданного именем
        Type atrType = Name.ToType();
        if (atrType == null)
        {
            throw new Exception($"Атрибут {Name} не зарегистрирован ");
        }
        if (atrType.IsExtendsFrom("Attribute")==false)
        {
            throw new Exception($"Тип {Name} не является атрибутом");
        }
        if (CustomTypeAttriburtes.ContainsKey(type.Name) == false)
        {
            CustomTypeAttriburtes[type.Name] = Utils.ForType(type);
        }
        if (CustomTypeAttriburtes[type.Name].ContainsKey(Name))
        {
            throw new Exception($"Атрибут {Name} уже определён для типа {type.Name}");
        }
        CustomTypeAttriburtes[type.Name][Name] = Value;
        CustomTypeAttributesCount++;
        return CustomTypeAttributesCount;
    }

    /// <summary>
    /// Добавление настраиваемого атрибута для типа
    /// </summary>
    /// <param name="name"></param>
    /// <param name="Value"></param>
    /// <returns></returns>
    public static int AddMemberAttr(Type type, string Member, string Name, string Value)
    {
        //проверяем наличие обьявления атрибута заданного именем
        Type atrType = Name.ToType();
        if (atrType == null)
        {
            throw new Exception($"Атрибут {Name} не зарегистрирован ");
        }
        if (atrType.IsExtendsFrom("Attribute")==false)
        {
            throw new Exception($"Тип {Name} не является атрибутом");
        }
        if (CustomMemberAttriburtes.ContainsKey(type.Name) == false)
        {
            CustomMemberAttriburtes[type.Name] = Utils.GetAttributesByMemberForType(type);
        }
        if (type.GetProperty(Member) == null && type.GetMethod(Member) == null)
        {
            throw new Exception($"Тип {type.Name} не определяет ни свойства ни метода с именем {Member}");
        }
        if (CustomMemberAttriburtes[type.Name].ContainsKey(Member) == false)
        {
            if (type.GetProperty(Member) != null)
            {
                CustomMemberAttriburtes[type.Name][Member] = Utils.ForProperty(type, Member);
            }
            else
            {
                CustomMemberAttriburtes[type.Name][Member] = Utils.ForMethod(type, Member);
            }
        }
        if (CustomMemberAttriburtes[type.Name][Member].ContainsKey(Name))
        {
            throw new Exception($"Элемент типа {type.Name} уже обьявляет атрибут {Name} для свойства (или метода) {Member}");
        }
        CustomMemberAttriburtes[type.Name][Member][Name] = Value;
        CustomMemberAttributesCount++;
        return CustomMemberAttributesCount;
    }


    /// <summary>
    /// Добавление настраиваемого атрибута Label для типа.
    /// Исп. краткого текстного наименования для контекнта
    /// </summary>
    public static int AddLabelForType(Type type, string Value)
    {
        try
        {
            if(LogCustomization)
                Console.WriteLine("Добавлен настраиваемый атрибут " +
                    nameof(EntityLabelAttribute) + " со значением [" + Value + "] для типа " + type.Name);
            int res = AddTypeAttr(type, nameof(EntityLabelAttribute), Value);
            if (Utils.LabelFor(type) != Value)
            {
                throw new Exception("Атрибут установлен не корректно");
            }
            return res++;

        }
        catch(Exception ex)
        {
            throw new Exception($"СustomAttributes.AddLabelForType({type}) => {ex.Message} \n {ex.ToString()}");
        }
    }
    

    

    /// <summary>
    /// Добавление настраиваемого атрибута Label для типа.
    /// Исп. краткого текстного наименования для контекнта
    /// </summary>
    public static int AddIconForType(Type type, string Value)
    {
        return AddTypeAttr(type, nameof(EntityIconAttribute), Value);
    }


    /// <summary>
    /// Инициаллизация свойств из значений атрибутов
    /// </summary>
    public class FromAttributes
    {
        /// <summary>
        /// Вывод сообщения об ошибке
        /// </summary>
        /// <param name="ex">исключение</param>
        public virtual void LogError(Exception ex)
        {
            Console.WriteLine($"[{GetType().GetName()}]: {ex.Message}");
            Console.WriteLine($"[{GetType().GetName()}]: {ex.StackTrace}");
        }

        /// <summary>
        /// Вывод информационного сообщения
        /// </summary>
        public virtual void Info(object item)
        {
            Console.WriteLine($"[{GetType().GetName()}]: {item}");
        }


        /// <summary>
        /// 
        /// </summary>  
        public void Init(Type type)
        {
            var attrs = type.GetAttrs();
            GetType().GetOwnPropertyNames().ToList().ForEach((name) => {
                string key = name + "Attribute";
                if (attrs.ContainsKey(key))
                {
                    TextDataSetter.SetValue(this, name, attrs[key]);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary> 
        public void Init(Type type, string prop)
        {
            var attrs = Utils.ForProperty(type, prop);
            GetType().GetOwnPropertyNames().ToList().ForEach((name) => {
                string key = name + "Attribute";
                if (attrs.ContainsKey(key))
                {
                    TextDataSetter.SetValue(this, name, attrs[key]);
                }
            });

        }
    }

}