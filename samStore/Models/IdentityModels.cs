using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace samStore.Models
{
    public class IdentityModels : IdentityDbContext<User>
    {
        public IdentityModels()
            : base("name=samStore")
        {

        }
    }

    public class User : IdentityUser
    {

    }
}