using System.Collections;
using System.Collections.Generic;
using SocialMediaMustBeGood2.Models;

namespace SocialMediaMustBeGood2.ViewModels
{
    public class PublicationsListViewModel
    { 
  
        public List<Publication> Publications { get; set; }
        public PageViewModel PageViewModel { get; set; }

        public List<List<Comment>> Comments { get; set; } 
    }
}
