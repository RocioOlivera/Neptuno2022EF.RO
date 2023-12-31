﻿using Neptuno2022EF.Entidades.Dtos.CtaCte;
using Neptuno2022EF.Entidades.Dtos.Venta;
using Neptuno2022EF.Entidades.Entidades;
using Neptuno2022EF.Entidades.Enums;
using Neptuno2022EF.Ioc;
using Neptuno2022EF.Servicios.Interfaces;
using Neptuno2022EF.Servicios.Servicios;
using Neptuno2022EF.Windows.Helpers;
using Neptuno2022EF.Windows.Helpers.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neptuno2022EF.Windows
{
    public partial class frmCtasCtes : Form
    {
        private readonly IServiciosCtasCtes _servicioCtaCte;
        private readonly IServiciosVentas _servicioVentas;
        private List<CtaCteListDto> lista;
        public frmCtasCtes(IServiciosCtasCtes servicios, IServiciosVentas serviciosVentas)
        {
            InitializeComponent();
            _servicioCtaCte = servicios;
            _servicioVentas = serviciosVentas;
        }

        private void frmCtasCtes_Load(object sender, EventArgs e)
        {
            try
            {
                lista = _servicioCtaCte.GetCtasCtes();
                MostrarDatosGrilla(lista);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void MostrarDatosGrilla(List<CtaCteListDto> lista)
        {
            dgvDatos.Rows.Clear();
            foreach (var item in lista)
            {
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(dgvDatos);
                SetearFila(r, item);
                AgregarFila(r);
            }
        }
        private void AgregarFila(DataGridViewRow r)
        {
            dgvDatos.Rows.Add(r);
        }

        private void SetearFila(DataGridViewRow r, CtaCteListDto item)
        {
            r.Cells[cmnCliente.Index].Value = item.NombreCliente;
            r.Cells[cmnSaldo.Index].Value = item.Saldo;
            if (item.Saldo > 0)
            {
                r.Cells[cmnSaldo.Index].Style.BackColor = Color.Red;
            }
            else if (item.Saldo <= 0)
            {
                r.Cells[cmnSaldo.Index].Style.BackColor = Color.Green;

            }
            r.Tag = item;
        }
        private void tsbDetalle_Click(object sender, EventArgs e)
        {

            if (dgvDatos.SelectedRows.Count == 0)
            {
                return;
            }
            var r = dgvDatos.SelectedRows[0];
            var cta = (CtaCteListDto)r.Tag;

            
            try
            {
                List<DetalleCtaCteListDto> ctaCteDetalleDto = _servicioCtaCte.GetDetalleCtasCtes(cta.ClienteId);
                frmDetalleCtaCte frm = new frmDetalleCtaCte(DI.Create<IServiciosCtasCtes>(),DI.Create<IServiciosClientes>()) { Text = "Detalle de la Cuenta" };
                frm.SetCtaCte(ctaCteDetalleDto);
                string nombre = cta.NombreCliente.ToString();
                frm.ShowDialog(this);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnCobrar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0)
            {
                return;
            }

            var r = dgvDatos.SelectedRows[0];
            var ctaCteDto = (CtaCteListDto)r.Tag;

            if (ctaCteDto.Saldo <= 0 )
            {
                return;
            }

            frmCobro frm = new frmCobro(DI.Create<IServiciosCtasCtes>(), DI.Create<IServiciosClientes>(), DI.Create<IServiciosVentas>()) { Text = "Introducir pago..." };
            frm.SetMonto(ctaCteDto.Saldo);
            DialogResult dr = frm.ShowDialog(this);
            var cta = _servicioCtaCte.GetCtaCtePorId(ctaCteDto.CtaCteId);
            try
            {
                var ctaCte = new CtaCte
                {
                    FechaMovimiento = DateTime.Now,
                    ClienteId = cta.ClienteId,
                    Debe = 0,
                    Haber = cta.Saldo,
                    Saldo = 0,
                    Movimiento = $"PAGO EFECT. Cta Cte"
                };

                _servicioCtaCte.Agregar(ctaCte);
                GridHelper.SetearFila(r, ctaCteDto);
                MostrarDatosGrilla(lista);
            }
            catch (Exception exception)
            {
                MessageHelper.Mensaje(TipoMensaje.Error, exception.Message, "Error");
            }
        }

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
