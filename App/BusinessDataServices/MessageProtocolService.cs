using ApplicationDb.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

 
public class MessageProtocolService
{
    private readonly BusinessDataModel _context;

    public MessageProtocolService( BusinessDataModel context )
    {
        _context = context;
    }

      /*
    public List<MessageProtocol> GetMessageProtocolsForUser(int userId)
    {
        UserContext  user = _context.Users.Include(u => u.UserGroups).Where(u => u.ID == userId).SingleOrDefault();

        user.Groups = (from g in _context.Groups where (from p in user.UserGroups select p.GroupID).Contains(g.ID) select g).ToList();
        var userGroupIDs = (from p in user.Groups select p.ID).ToList();
        var bsfs = (from p in _context.GroupsBusinessFunctions where userGroupIDs.Contains(p.GroupID) select p.BusinessFunctionID).ToList();
        var protocols = (from p in _context.MessageProtocols.Include(p => p.Properties) where p.FromID != null && bsfs.Contains((int)p.FromID) select p).ToList();
        return protocols.ToList();
    }


    
    public string ToSql(MessageProtocol model){
        return (new BusinessDatabaseGenerator().CreateDataModel(model).toSql());
    }

  
    public Form ToForm( int id )
    
    {                     
        MessageProtocol protocol = GetMessage(id);
        return ToForm(protocol);                
    }

    public Form ToForm(MessageProtocol protocol){
        Form form = new Form();
        form.Item = new Dictionary<string, object>();
        form.Chapter = protocol.Name;
        protocol.Properties.ForEach(prop =>
        {
            prop.Join("Attribute");
            var field = new FormField()
            {
                Label = prop.Label,
                Name = prop.Name,
                Help = prop.Help,
                Icon = prop.Attribute.Icon,
                Type = prop.Attribute.InputType               
            };
            Type type = ReflectionService.TypeForShortName($"Input{field.Type}Attribute");
            if( type != null)
            {
                field.CustomValidators[$"Input{field.Type}Attribute"] =
                    new List<object>() { null };
            }

            if (prop.Required)
            {
                field.CustomValidators[nameof(NotNullNotEmptyAttribute)] =
                    new List<object>() { "Свойство " + prop.Label + " нужно определить" };
            }
            if( prop.Unique)
            {
                field.CustomValidators[nameof(UniqValidationAttribute)] =
                    new List<object>() { "Свойство " + prop.Label + " должно иметь уникальное значение" };                
            }
            form.FormFields.Add(field);
        });



        var createButton = new Button()
        {
            Label = "Создать",
            OnClick = (button) =>
            {
                form.OnComplete(form);

            }
        };

        createButton.Bind("Enabled", form, "IsValid");
        createButton.Enabled = false;
        form.Buttons.Add(createButton);
        form.EnableChangeSupport = true;
        createButton.EnableChangeSupport = true;
        form.Update(form.PropertyNames = (from p in protocol.Properties select p.Name).ToArray()); ;
        form.FormFields.Reverse();
        form.Title = Attrs.LabelFor(protocol.GetLabel());
        form.Description = Attrs.DescriptionFor(protocol.GetDescription());
         
        return form;
    }*/


    /// <summary>
    /// Выбор сведений о протоколе передачи сообщений
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public MessageProtocol GetMessage( int id )
    {
        MessageProtocol protocol = (from p in _context.MessageProtocols where p.ID==id select p).SingleOrDefault();
        protocol.Properties = (from p in _context.MessageProperties.Include(p => p.Attribute) where p.MessageProtocolID == protocol.ID select p).ToList();
        if (protocol == null)
        {
            throw new Exception("Не найден протокол с идентификатором "+id);
        }        
        return protocol;
    }

    public MessageProperty AddProperty(int messageProtocolId, int messageAttributeId)
    {            
        var messageProtocol = _context.MessageProtocols.Find(messageProtocolId);
        if (messageProtocol == null)
        {
            throw new Exception();
        }
        MessageAttribute messageAttribute = _context.MessageAttributes.Find(messageAttributeId);
        if (messageAttribute == null)
        {
            throw new Exception();
        }
        var property = new MessageProperty()
        {
            Label = "Новое свойство",
            Required = false,
            Unique = false,
            Index = false,
            Help = messageAttribute.Description,
            AttributeID = messageAttributeId,
            Name = messageAttribute.Name,
            MessageProtocolID = messageProtocolId
        };
        _context.MessageProperties.Add(property);            
        _context.SaveChanges();
        return property;
    }

    public MessageAttribute GetMessageAttribute(int messageAttributeId)
    {
        return _context.MessageAttributes.Find(messageAttributeId);
    }

    public void UpdateMessageProperty(MessageProperty messageProperty)
    {
        _context.Update(messageProperty);
        _context.SaveChanges();
    }

    public MessageProperty GetMessageProperty(int messageProtocolId, int messagePropertyId)
    {
        MessageProperty property = _context.MessageProperties.Find(messagePropertyId);
        property.MessageProtocol = _context.MessageProtocols.Find(property.MessageProtocolID);
        return property;
    }
}
 