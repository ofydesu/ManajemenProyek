using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using RAB.Asset.Enum;
using RAB.Asset.Obyek;
using RAB.Data;
using RAB.Models.Utama;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Asset.OlahanModel
{
	public class QCekTitik
    {
		private readonly RabContext _context;
		private int _polaId;
		public QCekTitik(RabContext context, int polaId)
		{
			_context = context;
			_polaId = polaId;
		}
		public IQueryable<Titik> QryTblIni
        {
            get{
				return _context.TblTitik.Include(t=>t.Pola)
					.Where(t => t.PolaId == _polaId);
			}
		}
		private IQueryable<Titik> QryIniRaw
        {
            get{
				var qIni = QryTblIni.ToList();
				var qTtk = qIni.Select(t => new Titik
				{
					TtkId = t.TtkId,
					PolaId = t.PolaId,
					Pola = t.Pola,
					SumbuId = t.SumbuId,
					Sumbu = t.Sumbu,
					Nama = t.Nama,
					Jarak = t.Jarak,
					PosAbs = (from g2 in qIni
							  where g2.Sumbu == t.Sumbu && g2.SumbuId <= t.SumbuId
							  orderby g2.SumbuId
							  select g2.Jarak).Sum()
			}); 

				return qTtk.AsQueryable();
            }
        }
		private IQueryable<Titik> QryIni4ZRaw
        {
            get
            {
				var ttk = QryTblIni
							.Where(t => (int)t.Sumbu == (int)ESumbu.Z)
							.OrderByDescending(t => t.Jarak)
							.Select(t => new Titik
							{
								TtkId = t.TtkId,
								PolaId = t.PolaId,
								Pola = t.Pola,
								SumbuId = t.SumbuId,
								Sumbu = t.Sumbu,
								Nama = t.Nama,
								Jarak = t.Jarak,
								PosAbs = Math.Abs(t.Jarak),
							});
				return ttk;
			}
		}
		public IQueryable<Titik>QryIniBerposisi
        {
            get
            {
				var qIni = QryIniRaw.ToList();
				int maxPosY = 0;
                try{
					maxPosY = qIni.Where(t => t.Sumbu == ESumbu.Y).Select(s => s.PosAbs).Max();
                }
                catch { }

				var jRel = qIni.Select(r=> new Titik { 
					TtkId = r.TtkId,
					PolaId = r.PolaId,
					Pola = r.Pola,
					Nama = r.Nama,
					SumbuId = r.SumbuId,
					Sumbu = r.Sumbu,
					Jarak = r.Jarak,
					PosAbs = r.PosAbs,
					PosRel = (int)r.Sumbu == (int)ESumbu.Y ? maxPosY - r.PosAbs : r.PosAbs,
				});
				return jRel.AsQueryable();
            }
        }
		public IQueryable<Titik> QryIni4ZBerposisi
		{
			get
			{
				var qIni = QryIni4ZRaw.ToList();
				int maxPosZ = 0;
				int minPosZ = 0;
				try
				{
					maxPosZ = qIni.Select(s => s.Jarak).Max();
					minPosZ = qIni.Select(s => s.Jarak).Min();
				}
				catch { }
				int totPosZ = maxPosZ - minPosZ;

				var jRel = qIni.Select(r => new Titik
				{
					TtkId = r.TtkId,
					PolaId = r.PolaId,
					Pola = r.Pola,
					Nama = r.Nama,
					SumbuId = r.SumbuId,
					Sumbu = r.Sumbu,
					Jarak = r.Jarak,
					PosAbs = r.PosAbs,
					PosRel = totPosZ - r.Jarak + minPosZ
				});
				return jRel.AsQueryable();
			}
		}


	}
}
