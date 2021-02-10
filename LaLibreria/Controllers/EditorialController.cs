using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaLibreria.Models;
using Oracle.ManagedDataAccess.Client;

namespace LaLibreria.Controllers
{
    public class EditorialController : Controller
    {
        private readonly ModelContext _context;

        public EditorialController(ModelContext context)
        {
            _context = context;
        }

        // GET: Editorial
        public async Task<IActionResult> Index()
        {
            return View(await _context.Editorial.ToListAsync());
        }

        // GET: Editorial/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editorial = await _context.Editorial
                .FirstOrDefaultAsync(m => m.Ideditorial == id);
            if (editorial == null)
            {
                return NotFound();
            }

            return View(editorial);
        }

        // GET: Editorial/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Editorial/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ideditorial,NombreEditorial,Estado,FechaInsert")] Editorial editorial)
        {
            if (ModelState.IsValid)
            {
                using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
                {

                    OracleCommand objComando = new OracleCommand();
                    objComando.Connection = objConexion;
                    objComando.CommandText = "PRC_CREA_ACTUALIZA_EDITORIAL";
                    objComando.CommandType = System.Data.CommandType.StoredProcedure;
                    objComando.Parameters.Add("ideditorial", OracleDbType.Int16).Value = 0;
                    objComando.Parameters["ideditorial"].Size = 3;
                    objComando.Parameters.Add("nombreeditorial", OracleDbType.Varchar2).Value = editorial.NombreEditorial;
                    objComando.Parameters["nombreeditorial"].Size = 50;
                    objComando.Parameters.Add("codStatus", OracleDbType.Int16).Direction = System.Data.ParameterDirection.Output;
                    objComando.Parameters["codStatus"].Value = 500;
                    objComando.Parameters["codStatus"].Size = 3;
                    objComando.Parameters.Add("stringStatus", OracleDbType.NVarchar2).Direction = System.Data.ParameterDirection.Output;
                    objComando.Parameters["stringStatus"].Value = "Error. Editorial no encontrada para su actualización, verifique código interno.";
                    objComando.Parameters["stringStatus"].Size = 100;

                    try
                    {
                        objConexion.Open();
                        objComando.ExecuteNonQuery();
                        var codigo = objComando.Parameters["codStatus"].Value;

                        if (Convert.ToString(codigo) == "200")
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError("Editorial", Convert.ToString(objComando.Parameters["stringStatus"].Value));
                            return BadRequest(ModelState);
                        }
                        //objReader.NextResult();
                        //Console.WriteLine(objReader);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }
                    objConexion.Close();

                }
            }
            return View(editorial);
        }

        // GET: Editorial/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editorial = await _context.Editorial.FindAsync(id);
            if (editorial == null)
            {
                return NotFound();
            }
            return View(editorial);
        }

        // POST: Editorial/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Ideditorial,NombreEditorial,Estado,FechaInsert")] Editorial editorial)
        {
            if (id != editorial.Ideditorial)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
                {

                    OracleCommand objComando = new OracleCommand();
                    objComando.Connection = objConexion;
                    objComando.CommandText = "PRC_CREA_ACTUALIZA_EDITORIAL";
                    objComando.CommandType = System.Data.CommandType.StoredProcedure;
                    objComando.Parameters.Add("ideditorial", OracleDbType.Int16).Value = editorial.Ideditorial;
                    objComando.Parameters["ideditorial"].Size = 3;
                    objComando.Parameters.Add("nombreeditorial", OracleDbType.Varchar2).Value = editorial.NombreEditorial;
                    objComando.Parameters["nombreeditorial"].Size = 50;
                    objComando.Parameters.Add("codStatus", OracleDbType.Int16).Direction = System.Data.ParameterDirection.Output;
                    objComando.Parameters["codStatus"].Value = 500;
                    objComando.Parameters["codStatus"].Size = 3;
                    objComando.Parameters.Add("stringStatus", OracleDbType.NVarchar2).Direction = System.Data.ParameterDirection.Output;
                    objComando.Parameters["stringStatus"].Value = "Error. Editorial no encontrada para su actualización, verifique código interno.";
                    objComando.Parameters["stringStatus"].Size = 100;

                    try
                    {
                        objConexion.Open();
                        objComando.ExecuteNonQuery();
                        var codigo = objComando.Parameters["codStatus"].Value;

                        if (Convert.ToString(codigo) == "200")
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError("Editorial", Convert.ToString(objComando.Parameters["stringStatus"].Value));
                            return BadRequest(ModelState);
                        }
                        //objReader.NextResult();
                        //Console.WriteLine(objReader);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }
                    objConexion.Close();

                }
            }
            return View(editorial);
        }

        // GET: Editorial/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editorial = await _context.Editorial
                .FirstOrDefaultAsync(m => m.Ideditorial == id);
            if (editorial == null)
            {
                return NotFound();
            }

            return View(editorial);
        }

        // POST: Editorial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id, decimal estado)
        {
            using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
            {
                var nuevoEstado = 0;

                if (estado == 0)
                {
                    nuevoEstado = 1;
                }

                OracleCommand objComando = new OracleCommand();
                objComando.Connection = objConexion;
                objComando.CommandText = "PRC_DESHABILITA_EDITORIAL";
                objComando.CommandType = System.Data.CommandType.StoredProcedure;
                objComando.Parameters.Add("codEditorial", OracleDbType.Int16).Value = id;
                objComando.Parameters["codEditorial"].Size = 3;
                objComando.Parameters.Add("estadoEditorial", OracleDbType.Int16).Value = nuevoEstado;
                objComando.Parameters["estadoEditorial"].Size = 50;
                objComando.Parameters.Add("codStatus", OracleDbType.Int16).Direction = System.Data.ParameterDirection.Output;
                objComando.Parameters["codStatus"].Value = 500;
                objComando.Parameters["codStatus"].Size = 3;
                objComando.Parameters.Add("stringStatus", OracleDbType.NVarchar2).Direction = System.Data.ParameterDirection.Output;
                objComando.Parameters["stringStatus"].Value = "Error. Editorial no encontrada para su actualización, verifique código interno.";
                objComando.Parameters["stringStatus"].Size = 100;

                try
                {
                    objConexion.Open();
                    objComando.ExecuteNonQuery();
                    var codigo = objComando.Parameters["codStatus"].Value;

                    if (Convert.ToString(codigo) == "200")
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("Editorial", Convert.ToString(objComando.Parameters["stringStatus"].Value));
                        return BadRequest(ModelState);
                    }
                    //objReader.NextResult();
                    //Console.WriteLine(objReader);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Exception: {0}", ex.ToString());
                }
                objConexion.Close();

            }

            return RedirectToAction(nameof(Index));
        }

        private bool EditorialExists(decimal id)
        {
            return _context.Editorial.Any(e => e.Ideditorial == id);
        }
    }
}
