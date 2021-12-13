using System;
using System.Data.Odbc;


namespace WebApplication1
{
   public partial class Default : System.Web.UI.Page
   {
      protected void Page_Load(object sender, EventArgs e)
      {
         if (!IsPostBack) // загрузим данные только в первую загрузку страницы. При изменении данных в БД можно снова обновлять данные
         {
            Calendar1.SelectedDate = DateTime.Today;
            Label2.Text = "";
            Label3.Text = "";        
         }
      }
      
      protected void Button3_Click(object sender, EventArgs e)
      {        
         // Подключение БД и создание строки-запроса аналогично первому заданию
         OdbcConnection conn = new OdbcConnection();
         conn.ConnectionString = "Dsn=PostgreSQL30;database=students;server=postgresql.students.ami.nstu.ru;uid=pmi-b8307;sslmode=disable;readonly=0;protocol=7.4;fakeoidindex=0;showoidcolumn=0;rowversioning=0;showsystemtables=0;fetch=100;unknownsizes=0;maxvarcharsize=255;maxlongvarcharsize=8190;debug=0;commlog=0;usedeclarefetch=0;textaslongvarchar=1;unknownsaslongvarchar=0;boolsaschar=1;parse=0;lfconversion=1;updatablecursors=1;trueisminus1=0;bi=0;byteaaslongvarbinary=1;useserversideprepare=1;lowercaseidentifier=0;d6=-101;optionalerrors=0;fetchrefcursors=0;xaopt=1";
         try
         {
            conn.Open();
         }
         catch (Exception ex)
         {
            Label2.Text = ex.Message;
            return;
         }

         string strSQL = "UPDATE pmib8307.v set date_begin = (v.date_begin - interval '1 month')::date where pmib8307.v.n_v in (SELECT pmib8307.v.n_v from(SELECT pmib8307.v.n_izd, max(pmib8307.v.date_begin) as last_date from pmib8307.v group by pmib8307.v.n_izd) as t join pmib8307.v on pmib8307.v.n_izd = t.n_izd and pmib8307.v.date_begin = t.last_date join pmib8307.q on pmib8307.q.n_izd = t.n_izd where pmib8307.q.n_det = ?)";
         OdbcCommand cmd = new OdbcCommand(strSQL, conn);
         OdbcParameter par_name = new OdbcParameter();
         par_name.ParameterName = "@vdet";
         par_name.OdbcType = OdbcType.Text;
         par_name.Value = DropDownList3.SelectedValue;
         cmd.Parameters.Add(par_name);
         OdbcTransaction tx = null;
         try
         {
            tx = conn.BeginTransaction();
            cmd.Transaction = tx;
            // Выполняем SQL-команду и получаем кол-во обработанных строк
            var count_all = cmd.ExecuteNonQuery();
            Label2.Text = "Записей обработано: " + count_all;
            // Подтверждаем транзакцию  
            tx.Commit();
            //закрываем соединение
            conn.Close();
            //ShowInfo();
            //if (Label1.Text != "")
            //   ShowPrice();
            //DropDownList3_SelectedIndexChanged(sender, e);
         }
         catch (Exception ex)
         {
            // Формируем сообщение об ошибке 
            Label2.Text = ex.Message;
            // выполняем откат транзакции 
            tx.Rollback();
            //закрываем соединение
            conn.Close();
         }
         GridView1.DataBind();
         GridView2.DataBind();
      }

      // кнопки, обновляющие окно
      protected void UpdateForm(object sender, EventArgs e)
      {
         GridView1.DataBind();
         GridView2.DataBind();
      }
   }
}