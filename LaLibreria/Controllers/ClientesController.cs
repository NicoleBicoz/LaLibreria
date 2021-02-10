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
    public class ClientesController : Controller
    {
        private readonly ModelContext _context;

        public ClientesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientes = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Rut == id);
            if (clientes == null)
            {
                return NotFound();
            }

            return View(clientes);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idclientes,Nombres,Apellidos,Mail,Rut,FechaNacimiento,FechaInsert,Estado")] Clientes clientes)
        {
            if (ModelState.IsValid)
            {
                using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
                {

                    OracleCommand objComando = new OracleCommand();
                    objComando.Connection = objConexion;
                    objComando.CommandText = "PRC_CREA_ACTUALIZA_CLIENTES";
                    objComando.CommandType = System.Data.CommandType.StoredProcedure;
                    objComando.Parameters.Add("rutCliente", OracleDbType.Varchar2).Value = clientes.Rut;
                    objComando.Parameters["rutCliente"].Size = 10;
                    objComando.Parameters.Add("nombresCliente", OracleDbType.Varchar2).Value = clientes.Nombres;
                    objComando.Parameters["nombresCliente"].Size = 50;
                    objComando.Parameters.Add("apellidosCliente", OracleDbType.Varchar2).Value = clientes.Apellidos;
                    objComando.Parameters["apellidosCliente"].Size = 50;
                    objComando.Parameters.Add("mailCliente", OracleDbType.Varchar2).Value = clientes.Mail;
                    objComando.Parameters["mailCliente"].Size = 50;
                    objComando.Parameters.Add("fechaNacimiento", OracleDbType.Date).Value = clientes.FechaNacimiento;
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

                        objConexion.Close();
                        if (Convert.ToString(codigo) == "200")
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError("Cliente", Convert.ToString(objComando.Parameters["stringStatus"].Value));
                            return BadRequest(ModelState);
                        }
                        //objReader.NextResult();
                        //Console.WriteLine(objReader);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }

                }
                return RedirectToAction(nameof(Index));
            }
            return View(clientes);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(decimal id, string rut)
        {
            if (rut == null)
            {
                return NotFound();
            }

            var clientes = await _context.Clientes.FindAsync(rut, id);
            if (clientes == null)
            {
                return NotFound();
            }
            return View(clientes);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string rut, [Bind("Idclientes,Nombres,Apellidos,Mail,Rut,FechaNacimiento,FechaInsert,Estado")] Clientes clientes)
        {
            if (rut != clientes.Rut)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
                {

                    OracleCommand objComando = new OracleCommand();
                    objComando.Connection = objConexion;
                    objComando.CommandText = "PRC_CREA_ACTUALIZA_CLIENTES";
                    objComando.CommandType = System.Data.CommandType.StoredProcedure;
                    objComando.Parameters.Add("rutCliente", OracleDbType.Varchar2).Value = rut;
                    objComando.Parameters["rutCliente"].Size = 10;
                    objComando.Parameters.Add("nombresCliente", OracleDbType.Varchar2).Value = clientes.Nombres;
                    objComando.Parameters["nombresCliente"].Size = 50;
                    objComando.Parameters.Add("apellidosCliente", OracleDbType.Varchar2).Value = clientes.Apellidos;
                    objComando.Parameters["apellidosCliente"].Size = 50;
                    objComando.Parameters.Add("mailCliente", OracleDbType.Varchar2).Value = clientes.Mail;
                    objComando.Parameters["mailCliente"].Size = 50;
                    objComando.Parameters.Add("fechaNacimiento", OracleDbType.Date).Value = clientes.FechaNacimiento;
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

                        objConexion.Close();
                        if (Convert.ToString(codigo) == "200")
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError("Cliente", Convert.ToString(objComando.Parameters["stringStatus"].Value));
                            return BadRequest(ModelState);
                        }
                        //objReader.NextResult();
                        //Console.WriteLine(objReader);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }

                }
            }
            return View(clientes);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(string rut)
        {
            if (rut == null)
            {
                return NotFound();
            }

            var clientes = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Rut == rut);
            if (clientes == null)
            {
                return NotFound();
            }

            return View(clientes);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string rut, bool estado)
        {
            using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
            {
                var nuevoEstado = 0;

                if (estado == false)
                {
                    nuevoEstado = 1;
                }

                OracleCommand objComando = new OracleCommand();
                objComando.Connection = objConexion;
                objComando.CommandText = "PRC_DESHABILITA_CLIENTE";
                objComando.CommandType = System.Data.CommandType.StoredProcedure;
                objComando.Parameters.Add("rutCliente", OracleDbType.Varchar2).Value = rut;
                objComando.Parameters["rutCliente"].Size = 10;
                objComando.Parameters.Add("estadoCliente", OracleDbType.Int16).Value = nuevoEstado;
                objComando.Parameters["estadoCliente"].Size = 1;
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


                    objConexion.Close();

                    if (Convert.ToString(codigo) == "200")
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("Cliente", Convert.ToString(objComando.Parameters["stringStatus"].Value));
                        return BadRequest(ModelState);
                    }
                    //objReader.NextResult();
                    //Console.WriteLine(objReader);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Exception: {0}", ex.ToString());
                }

            }

            return RedirectToAction(nameof(Index));
        }

        private bool ClientesExists(string id)
        {
            return _context.Clientes.Any(e => e.Rut == id);
        }
    }
}
