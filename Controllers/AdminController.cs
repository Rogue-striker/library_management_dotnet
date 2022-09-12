using Microsoft.AspNetCore.Mvc;
public class AdminController : Controller{
    private IAdminDataBaseServices _adminDbService;
    private List<BookModel> _allBooks;
    private List<BookTransactionsModel> _allTransactions;
    public AdminController (IAdminDataBaseServices adminDbService){
        _adminDbService = adminDbService;
        _allBooks = new List<BookModel>();
        _allTransactions = new List<BookTransactionsModel>();
    }
    public IActionResult Index(){
         var verified = HttpContext.Session.GetInt32("verified");
         if(verified != 1){
            return RedirectToAction("userlogin","auth");
         }
        _allBooks = _adminDbService.GetAllBooks();
        return View(_allBooks);
    }
    public IActionResult AddNewBook(){
        return View();
    }
    [HttpPost]
    public IActionResult AddNewBook(BookModel newBook){
        if(_adminDbService.AddBook(newBook)){
            return RedirectToAction("addnewbook");
        }else{
            ViewBag.error = "true";
            return View("addnewbook");
        }
    }
    public IActionResult DeleteBook(int id){
        System.Console.WriteLine(id);
        var res = _adminDbService.DeleteBook(id);
        System.Console.WriteLine(res);
        if(res){
            return RedirectToAction("Index");
        }
        else{
            TempData["deleteError"] = "cannot delete that book";
            return RedirectToAction("Index");
        }
    }
    public IActionResult ShowTransactions(){
        _allTransactions  = _adminDbService.getAllTransactions();
        return View(_allTransactions);
    }
}