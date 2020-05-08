using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<IdentityUser> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(Roles roles)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole();
                role.Name = roles.Name;
                await _roleManager.CreateAsync(role);
            }

            return RedirectToAction("ViewRoles");
        }

        public IActionResult ViewRoles()
        {
            return View(_roleManager.Roles.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRoles(List<UserRoleViewModel> model)
        {
            string RoleName = "";
            IdentityResult result;
            bool tr = false;
            
            for (int x = 0; x < model.Count; x++)
            {
                IdentityUser usr = await _userManager.FindByNameAsync(model[x].Username);
                if (model[x].isSelected)
                {
                    if (!(await _userManager.IsInRoleAsync(usr, model[x].Role)))
                    {
                        tr = true;
                    }
                    result = await _userManager.AddToRoleAsync(usr, model[x].Role);
                    
                }
                else if (!model[x].isSelected && (await _userManager.IsInRoleAsync(usr, model[x].Role)))
                {
                    result = await _userManager.RemoveFromRoleAsync(usr, model[x].Role);
                }
                if (tr)
                {
                    Console.WriteLine("suc");
                }
                if (x == 0) RoleName = model[x].Role;
            }

            string Roleid = (await _roleManager.FindByNameAsync(RoleName)).Id;

            return RedirectToAction("AssignRoles", new { roleid = Roleid});
        }


        [HttpGet]
        public async Task<IActionResult> AssignRoles(string roleid)
        {
            var model = new List<UserRoleViewModel>();
            var rolename = (await _roleManager.FindByIdAsync(roleid)).Name;
            foreach(var item in _userManager.Users)
            {
                UserRoleViewModel userview = new UserRoleViewModel
                {
                    Username = item.UserName,
                    Role = rolename, 
                    isSelected = await _userManager.IsInRoleAsync(item, rolename)
                };

                model.Add(userview);
            }
            ViewBag.rolename = rolename;
            return View(model);
        }

        public IActionResult ViewUsers()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> ConfirmDelRole(String roleid)
        {
            IdentityRole delrole = await _roleManager.FindByIdAsync(roleid);
            return View(delrole);
        }

        public async Task<IActionResult> ConfirmDelUser (String usrId)
        {
            IdentityUser usr = await _userManager.FindByIdAsync(usrId);
            return View(usr);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(IdentityUser user)
        {

            IdentityUser usr =  await _userManager.FindByIdAsync(user.Id);
            IdentityResult result = await _userManager.DeleteAsync(usr);
            return RedirectToAction("ViewUsers");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(IdentityRole role)
        {

            IdentityRole rol = await _roleManager.FindByIdAsync(role.Id);
            IdentityResult result = await _roleManager.DeleteAsync(rol);
            return RedirectToAction("ViewRoles");
        }

        public async Task<IActionResult> ViewUserRoles(string usrId)
        {
            var usr = await _userManager.FindByIdAsync(usrId);
            if (usr == null)
            {
                Console.WriteLine(usrId);
                throw new Exception("USER NOT FOUND: ");
            }
            ViewBag.usr = usr;

            var roles = await _userManager.GetRolesAsync(usr);
            ViewBag.allroles = _roleManager.Roles.ToList();
            ViewBag.roles = roles;
            return View();
        }

        // public async Task<IActionResult> UserList(IdentityUser users)
        //{
        //  users.
        //}
    }
}