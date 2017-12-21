using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly CheeseDbContext context;

        public MenuController(CheeseDbContext DbContext)
        {
            context = DbContext;
        }

        public IActionResult Index()
        {
            List<Menu> menus = context.Menus.ToList();
            return View(menus);
        }

        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
            return View(addMenuViewModel);
        }


        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu {
                    Name = addMenuViewModel.Name
                };
                context.Menus.Add(newMenu);
                context.SaveChanges();
                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }
            return View(addMenuViewModel);
        }

        public IActionResult ViewMenu(int id)
        {
            List<CheeseMenu> items = context.CheeseMenus.Include(item => item.Cheese).Where(cm => cm.MenuID == id).ToList();
            Menu menu = context.Menus.Single(c => c.ID == id);
            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel {
                Items = items,
                Menu = menu
            };
            return View(viewMenuViewModel);
        }

        public IActionResult AddItem(int id)
        {
            Menu menu = context.Menus.Single(c => c.ID == id);
            //IEnumerable<Cheese> cheeses = context.Cheeses.ToList();
            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(menu, context.Cheeses.ToList());
            return View(addMenuItemViewModel);
        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            IList<CheeseMenu> existingItems = context.CheeseMenus
                .Where(cm => cm.CheeseID == addMenuItemViewModel.cheeseID)
                .Where(cm => cm.MenuID == addMenuItemViewModel.menuID).ToList();

            if (existingItems.Count == 0)
            {
                CheeseMenu cheeseMenu = new CheeseMenu {
                    MenuID = addMenuItemViewModel.menuID,
                    CheeseID = addMenuItemViewModel.cheeseID,
                    Menu = addMenuItemViewModel.Menu
                };

                context.CheeseMenus.Add(cheeseMenu);
                context.SaveChanges();
                return Redirect("/Menu/ViewMenu/" + addMenuItemViewModel.menuID);


            }
            return Redirect("/Menu/ViewMenu/" + addMenuItemViewModel.menuID);
        }
    }
}