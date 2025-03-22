using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreMvc.Models;
using ShoeStoreMvc.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoeStoreMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        // Trang đăng nhập GET
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        // Xử lý đăng ký tài khoản mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(ShoeStoreMvc.ViewModels.RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Tạo ApplicationUser từ RegisterViewModel
                var user = new ApplicationUser
                {
                    UserName = model.Username,  // Dùng Username thay vì Email
                    Email = model.Email
                };

                // Tạo tài khoản mới
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {

                    // Đăng nhập ngay sau khi tạo tài khoản
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");  // Redirect đến trang chủ sau khi đăng ký
                }
                else
                {
                    // Thêm lỗi vào ModelState nếu tạo tài khoản thất bại
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);  // Trả về lại view nếu có lỗi
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // Kiểm tra đăng nhập
                var userUsername = await _userManager.FindByEmailAsync(model.Email);
                if (userUsername == null)
                {
                    ModelState.AddModelError(string.Empty, "Email không tồn tại.");
                    return View(model);
                }

                // Dùng UserName để đăng nhập thay vì Email
                var result = await _signInManager.PasswordSignInAsync(userUsername.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // Lấy thông tin người dùng từ UserManager
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    // Lưu thông tin người dùng vào ViewData để hiển thị trên các View (ví dụ như avatar, username)
                    ViewData["UserName"] = user.UserName;
                    ViewData["AvatarUrl"] = user.Avatar; // Giả sử bạn đã lưu URL avatar trong thuộc tính Avatar của người dùng

                    // Chuyển hướng đến trang gốc hoặc trang được yêu cầu
                    return LocalRedirect(returnUrl); // Redirect về trang trước đó
                }
                else
                {
                    // Thêm lỗi vào ModelState nếu đăng nhập thất bại
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");  // Redirect về trang chủ sau khi đăng xuất
        }



        // Route xử lý đăng nhập bằng Google
        [HttpGet]
        [Route("Account/LoginWithGoogle")]
        public async Task<IActionResult> LoginWithGoogle(string returnUrl = null)
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(properties, "Google");
        }

        // Route xử lý đăng nhập bằng Facebook
        [HttpGet]
        [Route("Account/LoginWithFacebook")]
        public async Task<IActionResult> LoginWithFacebook(string returnUrl = null)
        {
            var redirectUrl = Url.Action("FacebookResponse", "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return Challenge(properties, "Facebook");
        }

        // Nhận phản hồi từ Google
        public async Task<IActionResult> GoogleResponse(string returnUrl = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) return RedirectToAction("Login");

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var avatarUrl = info.Principal.FindFirstValue("picture");
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser { UserName = email, Email = email, Avatar = avatarUrl };
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    foreach (var error in createResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return RedirectToAction("Login");
                }

                await _userManager.AddLoginAsync(user, info);
            }
            else if (string.IsNullOrEmpty(user.Avatar))
            {
                user.Avatar = avatarUrl;
                await _userManager.UpdateAsync(user);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            // ✅ Lưu Avatar vào Session
            HttpContext.Session.SetString("UserAvatar", user.Avatar);
            HttpContext.Session.SetString("UserName", user.UserName);

            return RedirectToAction("Index", "Home");
        }

        // Nhận phản hồi từ Facebook
        public async Task<IActionResult> FacebookResponse(string returnUrl = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) return RedirectToAction("Login");

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            // Lấy avatar từ Facebook (Facebook không trả về "picture" như Google, ta phải lấy từ Graph API)
            var facebookId = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var avatarUrl = $"https://graph.facebook.com/{facebookId}/picture?type=large";

            if (user == null)
            {
                user = new ApplicationUser { UserName = email, Email = email, Avatar = avatarUrl };
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    foreach (var error in createResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return RedirectToAction("Login");
                }

                await _userManager.AddLoginAsync(user, info);
            }
            else if (string.IsNullOrEmpty(user.Avatar))
            {
                user.Avatar = avatarUrl;
                await _userManager.UpdateAsync(user);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            // ✅ Lưu Avatar vào Session
            HttpContext.Session.SetString("UserAvatar", user.Avatar);
            HttpContext.Session.SetString("UserName", user.UserName);

            return RedirectToAction("Index", "Home");
        }


    }
}
