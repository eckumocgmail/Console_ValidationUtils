
using Console_Encoder;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AuthorizationServices: AuthorizationCollection<Service>, APIWebServices
{

    private byte[] PrivateKey;

    public AuthorizationServices(ILogger<AuthorizationCollection<Service>> logger, AuthorizationOptions options) : base(logger, options)
    {
    }

    
    public void Authenticate(byte[] privateKey)
    {
        var bitles = new Console_Encoder.BitConverter();
        var data = new List<bool>();
        foreach (byte ch in privateKey)
            data.AddRange(bitles.ToBinary(ch));
        var decoder = new CharDecoder(data);
        Console.WriteLine(decoder.Decode());
    }

 
    public byte[] Publish(Service service)
    {

        return service.PublicKey;
    }
}