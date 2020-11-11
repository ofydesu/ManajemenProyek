using Microsoft.EntityFrameworkCore;
using RAB.Asset.Obyek;
using RAB.Asset.OlahanModel;
using RAB.Data;
using RAB.Models.Utama;
//using System;
//using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;

namespace RAB.BisnisModel.OlahanModel
{
    public class QCekKompKoordinat
    {
		private readonly RabContext _context;
		private int _polaId;
		//private int _koorId;
		public QCekKompKoordinat(RabContext context, int polaId)
		{
			_context = context;
			_polaId = polaId;
		}
		public IQueryable<KomponenKoordinat> QryTblIni
		{
			get
			{
				var qIni = _context.TblKomponenKoordinat
							.Include(k=>k.Koordinat)
								.Include(k => k.Koordinat.TitikX)
								.Include(k => k.Koordinat.TitikY)
							.Include(k=>k.PolaKomponen)
							.Where(k=>k.PolaKomponen.PolaId == _polaId)
							;
				return qIni;
			}
		}

		public IQueryable<OGaris> QryGarisPola(int koorId)
		{
			var kompIni = QryTblIni.Where(k => k.KoorId == koorId).First();
			QCekBidangDatar qDatar = new QCekBidangDatar(_context, kompIni.PolaKomponen.Komponen.PolaId);
			var qGaris = qDatar.QryGarisPartial.OrderBy(g => g.Arah);

			return qGaris.AsQueryable();
		}
	}
}
