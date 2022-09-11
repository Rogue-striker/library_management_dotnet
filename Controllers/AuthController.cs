using Microsoft.AspNetCore.Mvc;
public class AuthController : Controller{
    private IAuthDatabaseService _AuthService;
    public AuthController(IAuthDatabaseService authService)
    {
        _AuthService = authService;
    }
    public IActionResult UserLogin()
    {
        return View();
    }
    [HttpPost]
    public IActionResult UserLogin(UserModel credentials){
        if (credentials.userName == "admin"){
            if (credentials.password == "admin"){
                HttpContext.Session.SetInt32("verified", 1);
                return RedirectToAction(actionName: "Index",controllerName: "Admin");
            }
        }
        else{
            var userid = _AuthService.Login(credentials);
            if(userid != -1){
                HttpContext.Session.SetInt32("userid", userid);
                return RedirectToAction("Index", "User");
            }
        }
       return RedirectToAction("UserLogin");
    }

    public IActionResult Register(){
        return View();
    }
    [HttpPost]
    public IActionResult Register(UserModel userDetails){
        int userID = _AuthService.Register(userDetails);
        if (userID != -1)
            return RedirectToAction("confirmation", new { userID });
        else
            return RedirectToAction("Register");
    }
    public IActionResult Confirmation(int userid){
        return View(userid);
    }
    public IActionResult Logout(){
        if(HttpContext.Session.GetInt32("userid") !=null)
            HttpContext.Session.Remove("userid");
        if(HttpContext.Session.GetInt32("verified") !=null)
            HttpContext.Session.Remove("verified");
        return RedirectToAction("Index", "Home");
    }
}
