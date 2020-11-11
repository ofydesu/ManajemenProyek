using Microsoft.EntityFrameworkCore;
using RAB.Data;
using RAB.Models.Utama;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Asset.OlahanModel
{
    public class QCekGaris
    {
		private readonly RabContext _context;
		private int _polaId;
		public QCekGaris(RabContext context, int polaId)
		{
			_context = context;
			_polaId = polaId;
		}

		private IQueryable<Garis> QTblRaw
		{
			get
			{
				var grs = _context.TblGaris
							.Include(g => g.KoordAwal)
							.Include(g => g.KoordAkhir)
							.Include(g => g.KoordAwal.TitikX)
							.Include(g => g.KoordAwal.TitikY)
							.Include(g => g.KoordAkhir.TitikX)
							.Include(g => g.KoordAkhir.TitikY);
				return grs;
			}
		}
		public IQueryable<Garis> QryTblIni
		{
			get
			{


				var qIni = QTblRaw	//.Include(g => g.KoordAkhir).Include(g => g.KoordAwal)
							.Where(t => t.KoordAwal.TitikX.PolaId == _polaId)
							.ToList();
				return qIni.AsQueryable();
			}
		}
		public IQueryable<Garis> QryTblIniPosUpToDate
        {
            get
            {
				QCekKoordinat qcKoord = new QCekKoordinat(_context, _polaId);
				var qkoorUp = qcKoord.QryTblIniPosUpToDate;
				var qIni = from g in QryTblIni
						   join ks in qkoorUp on g.AwalId equals ks.KoordId
						   join ke in qkoorUp on g.AkhirId equals ke.KoordId
						   select new Garis()
						   {
							   GarisId = g.GarisId,
							   KoordAwal = ks,
							   KoordAkhir = ke,
							   Arah = g.Arah,							   
						   };
				return qIni;
            }
        }
		private IQueryable<Garis> ParseDariOGaris
		{
			get
			{
				QCekBidangDatar qDatar = new QCekBidangDatar(_context, _polaId);
				var qBaruDariGambar = qDatar.QryGarisPartial.ToList();
					var grs = qBaruDariGambar.Select(g => new Garis
					{
						AwalId = g.Awal.KoordId,
						AkhirId =g.Akhir.KoordId,
						Arah = g.Arah,
						KoordAwal = g.Awal,
						KoordAkhir = g.Akhir
					
					});
					return grs.AsQueryable();
			}

		}
		public void UpdateDariGambar()
			{
				var qBaruDariGambar = ParseDariOGaris.ToList();

				var lstTblIni = QryTblIni.ToList();
				var qLamaUntukDihapus = lstTblIni.Where(l => !qBaruDariGambar.Select(g => g.Nama).Contains(l.Nama)).ToList();

				var qBaruUntukDitambahkan = qBaruDariGambar.Where(b => !lstTblIni.Select(h => h.Nama).Contains(b.Nama)).ToList();

				try
				{
					if (qLamaUntukDihapus.Count > 0)
					{
						_context.TblGaris.RemoveRange(qLamaUntukDihapus);
						_context.SaveChanges();
					}
				}
				catch (Exception) { }

				try
				{
					if (qBaruUntukDitambahkan.Count > 0)
					{
						//_context.TblGaris.AddRange(qBaruUntukDitambahkan);
						foreach (var grs in qBaruUntukDitambahkan)
						{
							_context.TblGaris.Add(new Garis()
							{
								AwalId = grs.AwalId,
								AkhirId = grs.AkhirId,
								Arah = grs.Arah
							});
						}
						_context.SaveChanges();
					}
				}
				catch (Exception) { }
			}
	}
}
