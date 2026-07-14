using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Exceptions.UnAuthorizeException
{
    public class UnAuthorizedException() : Exception("Somthing went wrong maybe email or password invalid")
    {
        
    }
}