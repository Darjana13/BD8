using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication1
{
   public partial class Default : System.Web.UI.Page
   {
      protected void Page_Load(object sender, EventArgs e)
      {
         if (!IsPostBack) // загрузим данные только в первую загрузку страницы. При изменении данных в БД можно снова обновлять данные
         {
            Calendar1.SelectedDate = DateTime.Today;
            Label1.Text = "";
            Label2.Text = "";
            Label3.Text = "";

            string strSQL = "SELECT pmib8307.p.name, pmib8307.p.n_det from pmib8307.p";
            // Настроим отображаемые данные и данные-значение в выпадающем списке
            DropDownList3.DataSource = Load_List_Of_Details(strSQL);
            DropDownList3.DataTextField = "name_and_n_det";
            DropDownList3.DataValueField = "n_det";
            // Обновим данные в соответствии с новым источником
            DropDownList3.DataBind();
            // По умолчанию выбран первый элемент
            DropDownList3.SelectedIndex = 0;

            // то же самое для изделий
            strSQL = "SELECT pmib8307.j.name, pmib8307.j.n_izd from pmib8307.j";
            DropDownList1.DataSource = Load_List_Of_Details(strSQL);
            DropDownList1.DataTextField = "name_and_n_det";
            DropDownList1.DataValueField = "n_det";
            DropDownList1.DataBind();
            DropDownList1.SelectedIndex = 0;
         }
      }
      DataView Load_List_Of_Details(string strSQL )
      {
         // Создадим таблицу, чтобы хранить данные для выпадающего списка
         DataTable details = new DataTable();
         // Определим названия колонок
         details.Columns.Add(new DataColumn("name_and_n_det", typeof(string)));
         details.Columns.Add(new DataColumn("n_det", typeof(string)));

         // Заполним таблицу модифицированными даннами из БД
         OdbcConnection conn = new OdbcConnection();
         conn.ConnectionString = "Dsn=PostgreSQL30;database=students;server=postgresql.students.ami.nstu.ru;uid=pmi-b8307;sslmode=disable;readonly=0;protocol=7.4;fakeoidindex=0;showoidcolumn=0;rowversioning=0;showsystemtables=0;fetch=100;unknownsizes=0;maxvarcharsize=255;maxlongvarcharsize=8190;debug=0;commlog=0;usedeclarefetch=0;textaslongvarchar=1;unknownsaslongvarchar=0;boolsaschar=1;parse=0;lfconversion=1;updatablecursors=1;trueisminus1=0;bi=0;byteaaslongvarbinary=1;useserversideprepare=1;lowercaseidentifier=0;d6=-101;optionalerrors=0;fetchrefcursors=0;xaopt=1";
         try
         {
            conn.Open();
         }
         catch (Exception ex)
         { 
            Label3.Text = ex.Message;
            // создаем представленее, понятное для выподающего списка
            DataView dv_err = new DataView(details);
            return dv_err;
         }
         OdbcCommand cmd = new OdbcCommand(strSQL, conn);
         OdbcTransaction tx = null;
         try
         {
            tx = conn.BeginTransaction();
            cmd.Transaction = tx;
            var cursor = cmd.ExecuteReader();
            while (cursor.Read())
            {
               string name = (string)cursor.GetValue(0); // запомним значение первого столбца
               string n_det = (string)cursor.GetValue(1); // и второго
               DataRow dr = details.NewRow(); // создадим пустую таблицу для данных
               dr[0] = name + " - " + n_det; // отображаемые дынные "Название - Номер"
               dr[1] = n_det; // значащие данные "Номер"
               details.Rows.Add(dr); // добавим полученную строку в таблицу
            }
            cursor.Close(); 
            tx.Commit();
         }
         catch (Exception ex)
         {
            Label3.Text = ex.Message;
            tx.Rollback();
         }
         conn.Close();

         // создаем представленее, понятное для выподающего списка
         DataView dv = new DataView(details);
         return dv;
      }

      protected void Button1_Click(object sender, EventArgs e)
      {
         

         /* запрос
         SELECT * 
         from pmib8307.v 
         where n_izd = 'J1'
         and date_begin < '2011-07-01'
         order by date_begin desc
         limit(1)
          */
         ShowPrice();

      }

      void ShowPrice()
      {
         // Создаем объект подключения
         OdbcConnection conn = new OdbcConnection();
         // Задаем параметр подключения – имя ODBC-источника
         conn.ConnectionString = "Dsn=PostgreSQL30;database=students;server=postgresql.students.ami.nstu.ru;uid=pmi-b8307;sslmode=disable;readonly=0;protocol=7.4;fakeoidindex=0;showoidcolumn=0;rowversioning=0;showsystemtables=0;fetch=100;unknownsizes=0;maxvarcharsize=255;maxlongvarcharsize=8190;debug=0;commlog=0;usedeclarefetch=0;textaslongvarchar=1;unknownsaslongvarchar=0;boolsaschar=1;parse=0;lfconversion=1;updatablecursors=1;trueisminus1=0;bi=0;byteaaslongvarbinary=1;useserversideprepare=1;lowercaseidentifier=0;d6=-101;optionalerrors=0;fetchrefcursors=0;xaopt=1";
         // Подключаемся к БД
         try
         {
            conn.Open();
         }
         catch (Exception ex)
         {
            // Формируем сообщение об ошибке 
            Label1.Text = ex.Message;
            return;
         }
         // Определяем строку с текстом запроса
         string strSQL = "SELECT cost from pmib8307.v where n_izd = ? and  date_begin < ? order by date_begin desc limit(1)";
         // Создаем объект запроса
         OdbcCommand cmd = new OdbcCommand(strSQL, conn);
         // Создаем первый параметр
         OdbcParameter par_name = new OdbcParameter();
         par_name.ParameterName = "@vizd";
         par_name.OdbcType = OdbcType.Text;
         par_name.Value = DropDownList1.SelectedValue;
         cmd.Parameters.Add(par_name); // Добавляем первый параметр в коллекцию
         // Создаем второй параметр
         OdbcParameter par_date = new OdbcParameter();
         par_date.ParameterName = "@vdate";
         par_date.OdbcType = OdbcType.Date;
         par_date.Value = Calendar1.SelectedDate.ToShortDateString();
         cmd.Parameters.Add(par_date); // Добавляем второй параметр в коллекцию.
         // Объявляем объект транзакции
         OdbcTransaction tx = null;
         try
         {
            // Начинаем транзакцию и извлекаем объект транзакции из объекта подключения.
            tx = conn.BeginTransaction();
            // Включаем объект SQL-команды в транзакцию
            cmd.Transaction = tx;
            // Выполняем SQL-команду и получаем единственное значение
            var res = cmd.ExecuteScalar();
            if (res is null)
               Label1.Text = "Для изделия \"" + DropDownList1.Items[DropDownList1.SelectedIndex].Text +
                  "\" на дату " + Calendar1.SelectedDate.ToShortDateString() +
                  " не задана рекомендованная цена"; // выводим результат
            else
               Label1.Text = "Рекомендованная цена: " + res; // выводим результат
            // Подтверждаем транзакцию  
            tx.Commit();
         }
         catch (Exception ex)
         {
            // При возникновении любой ошибки 
            // Формируем сообщение об ошибке 
            Label1.Text = ex.Message;
            // выполняем откат транзакции 
            tx.Rollback();
         }
         //закрываем соединение
         conn.Close();
      }
      public class Row1_type
      {
         public string name_izd { get; set; }
         public string date { get; set; }
         public int cost { get; set; }
         public string name_det { get; set; }
         public Row1_type(string name_izd, string date_begin, int cost, string name_det)
         {
            this.name_izd = name_izd;
            this.date = date_begin;
            this.cost = cost;
            this.name_det = name_det;
         }
      }

      protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
      {
         /* запрос показать как сейчас для детали Р1
          SELECT pmib8307.j.name, pmib8307.j.n_izd, t.last_date, pmib8307.v.cost, pmib8307.p.name, pmib8307.p.n_det
          from pmib8307.j
          join (select pmib8307.v.n_izd, max(pmib8307.v.date_begin) as last_date
                from pmib8307.v
                group by pmib8307.v.n_izd
               )t on t.n_izd = pmib8307.j.n_izd
          join pmib8307.q on pmib8307.q.n_izd = pmib8307.j.n_izd
          join pmib8307.p on pmib8307.p.n_det = pmib8307.q.n_det
          join pmib8307.v on t.n_izd = pmib8307.v.n_izd and pmib8307.v.date_begin=t.last_date
          where pmib8307.p.n_det = 'P1'
          */
         ShowInfo();
      }

      void ShowInfo()
      {
         var result = new List<Row1_type>();
         // Подключение БД и создание строки-запроса аналогично первому заданию
         OdbcConnection conn = new OdbcConnection();
         conn.ConnectionString = "Dsn=PostgreSQL30;database=students;server=postgresql.students.ami.nstu.ru;uid=pmi-b8307;sslmode=disable;readonly=0;protocol=7.4;fakeoidindex=0;showoidcolumn=0;rowversioning=0;showsystemtables=0;fetch=100;unknownsizes=0;maxvarcharsize=255;maxlongvarcharsize=8190;debug=0;commlog=0;usedeclarefetch=0;textaslongvarchar=1;unknownsaslongvarchar=0;boolsaschar=1;parse=0;lfconversion=1;updatablecursors=1;trueisminus1=0;bi=0;byteaaslongvarbinary=1;useserversideprepare=1;lowercaseidentifier=0;d6=-101;optionalerrors=0;fetchrefcursors=0;xaopt=1";
         //conn.ConnectionString = "Dsn=PostgreSQL30;database=students;server=postgresql.students.ami.nstu.ru;uid=pmi-b8305;sslmode=disable;readonly=0;protocol=7.4;fakeoidindex=0;showoidcolumn=0;rowversioning=0;showsystemtables=0;fetch=100;unknownsizes=0;maxvarcharsize=255;maxlongvarcharsize=8190;debug=0;commlog=0;usedeclarefetch=0;textaslongvarchar=1;unknownsaslongvarchar=0;boolsaschar=1;parse=0;lfconversion=1;updatablecursors=1;trueisminus1=0;bi=0;byteaaslongvarbinary=1;useserversideprepare=1;lowercaseidentifier=0;d6=-101;optionalerrors=0;fetchrefcursors=0;xaopt=1";

         try
         {
            conn.Open();
         }
         catch (Exception ex)
         {
            Label2.Text = ex.Message;
            return;
         }
         bool cursor_empty = false;
         string strSQL = "SELECT pmib8307.j.name, pmib8307.j.n_izd, t.last_date, pmib8307.v.cost, pmib8307.p.name, pmib8307.p.n_det from pmib8307.j join pmib8307.q on pmib8307.q.n_izd = pmib8307.j.n_izd join pmib8307.p on pmib8307.p.n_det = pmib8307.q.n_det join (select pmib8307.v.n_izd, max(pmib8307.v.date_begin) as last_date from pmib8307.v group by pmib8307.v.n_izd) t on t.n_izd = pmib8307.j.n_izd join pmib8307.v on t.n_izd = pmib8307.v.n_izd and pmib8307.v.date_begin = t.last_date where pmib8307.p.n_det = ? ";
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
            // Выполняем SQL-команду и получаем курсор на активное множество
            var cursor = cmd.ExecuteReader();
            if (cursor.HasRows)
            {// сохраняем результат запроса в список
               while (cursor.Read())
               {
                  result.Add(new Row1_type(
                     (string)cursor.GetValue(0) + " - " + (string)cursor.GetValue(1),
                     ((DateTime)cursor.GetValue(2)).ToShortDateString(),
                     (int)cursor.GetValue(3),
                     (string)cursor.GetValue(4) + " - " + (string)cursor.GetValue(5))
                     );
               }
               Label2.Text = "";
            }
            else
            {
               cursor_empty = true;
               Label2.Text = "Данных не найдено";
            }
            // Закрываем курсор
            cursor.Close();
            // Подтверждаем транзакцию  
            tx.Commit();
         }
         catch (Exception ex)
         {
            // Формируем сообщение об ошибке 
            Label2.Text = ex.Message;
            // выполняем откат транзакции 
            tx.Rollback();
         }

         //закрываем соединение
         conn.Close();

         if (!cursor_empty)
         {
            GridView1.DataSource = result;
            GridView1.DataBind();
            GridView1.HeaderRow.Cells[0].Text = "Изделие";
            GridView1.HeaderRow.Cells[1].Text = "Дата";
            GridView1.HeaderRow.Cells[2].Text = "Цена";
            GridView1.HeaderRow.Cells[3].Text = "Деталь";
         }
         else
         {
            GridView1.DataSource = null;
            GridView1.DataBind();
         }
      }

      protected void Button3_Click(object sender, EventArgs e)
      {
         /*
          // ??? - если дата на месяц назад уже есть, надо ли что-то делать?
          UPDATE pmib8307.v set date_begin=(v.date_begin - interval '1 month')::date
          where (pmib8307.v.n_izd,pmib8307.v.date_begin) in
               (SELECT pmib8307.j.n_izd, t.last_date
                from pmib8307.j
                join (select pmib8307.v.n_izd, max(pmib8307.v.date_begin) as last_date
                      from pmib8307.v
                      group by pmib8307.v.n_izd
                      )t on t.n_izd = pmib8307.j.n_izd
                join pmib8307.q on pmib8307.q.n_izd = pmib8307.j.n_izd
                join pmib8307.p on pmib8307.p.n_det = pmib8307.q.n_det
                join pmib8307.v on t.n_izd = pmib8307.v.n_izd and pmib8307.v.date_begin=t.last_date
                where pmib8307.p.n_det = 'P1'
                )
          */

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

         string strSQL = "UPDATE pmib8307.v set date_begin=(v.date_begin - interval '1 month')::date where(pmib8307.v.n_izd, pmib8307.v.date_begin) in (SELECT pmib8307.j.n_izd, t.last_date from pmib8307.j join(select pmib8307.v.n_izd, max(pmib8307.v.date_begin) as last_date from pmib8307.v group by pmib8307.v.n_izd) t on t.n_izd = pmib8307.j.n_izd join pmib8307.q on pmib8307.q.n_izd = pmib8307.j.n_izd join pmib8307.p on pmib8307.p.n_det = pmib8307.q.n_det join pmib8307.v on t.n_izd = pmib8307.v.n_izd and pmib8307.v.date_begin = t.last_date where pmib8307.p.n_det = ? )";
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
            ShowInfo();
            if (Label1.Text != "")
               ShowPrice();
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

        
      }
   }
}