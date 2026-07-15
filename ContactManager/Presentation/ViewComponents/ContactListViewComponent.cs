using ContactManager.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.Presentation.ViewComponents
{
    public class ContactListViewComponent : ViewComponent
    {
        private readonly IContactRepository _contactRepository;

        public ContactListViewComponent(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var contacts = await _contactRepository.GetAllAsync();
            
            return View(contacts);
        }
    }
}
