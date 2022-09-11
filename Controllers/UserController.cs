using Microsoft.AspNetCore.Mvc;
public class UserController : Controller{
    private ILibraryDataBaseService _UserDbService;
    public UserController(ILibraryDataBaseService UserDbService){
        _UserDbService = UserDbService;
    }
    public IActionResult Index(){
        if (HttpContext.Session.GetInt32("userid") != null){
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("userid"));
            System.Console.WriteLine(UserId);
            List<BookTransactionsModel> allUserBooks = _UserDbService.getUserTransactions(UserId);
            System.Console.WriteLine(allUserBooks.Count);
            return View(allUserBooks);
        }
        else{
            return RedirectToAction("Index", "Home");
        }
    }
    public IActionResult LendBook(){
        if (HttpContext.Session.GetInt32("userid") != null){
            List<BookModel> availableBooks = _UserDbService.showBooks();
            return View(availableBooks);
        }
        else{
            return RedirectToAction("Index", "Home");
        }
    }
    public IActionResult LendNewBook(int bookid){
        if (HttpContext.Session.GetInt32("userid") != null){
            int userid = Convert.ToInt32(HttpContext.Session.GetInt32("userid"));
            UserLendBookModel newBook = new UserLendBookModel(userid,bookid);
            if(_UserDbService.lendBook(newBook)){
               return RedirectToAction("LendBook");
            }
            else{
               return RedirectToAction("lendBook");
            }
        }
        else{
            return RedirectToAction("Index", "Home");
        }
    }
    public IActionResult ReturnBook(int bookTransactionId){
        if (HttpContext.Session.GetInt32("userid") != null){
            _UserDbService.returnBook(bookTransactionId);

            return RedirectToAction("Index");
        }
        else{
            return RedirectToAction("Index", "Home");
         }

      }
}