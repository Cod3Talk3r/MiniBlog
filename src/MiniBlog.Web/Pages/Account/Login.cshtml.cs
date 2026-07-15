using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlog.Application.Interfaces;
using MiniBlog.Domain.Entities;

namespace MiniBlog.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public LoginModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public class InputModel
        {
            public string Email { get; set; } = default!;
            public string Password { get; set; } = default!;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user is null)
            {
                ErrorMessage = "Email Or Password is Wrong!";
                return Page();
            }

            var check = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, lockoutOnFailure: false);
            if (!check.Succeeded)
            {
                ErrorMessage = "Email Or Password is Wrong!";
                return Page();
            }

            var token = _tokenService.GenerateToken(user);
            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return RedirectToPage("/Posts/Index");
        }
    }
}