using Microsoft.EntityFrameworkCore;
using RAB.Asset.Obyek;
using RAB.Asset.OlahanModel;
using RAB.Data;
using RAB.Models.Utama;
using System.Collections.Generic;
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
							//.Where(k=>k.PolaKomponen.PolaId == _polaId)
							;
				return qIni;
			}
		}
		public KomponenKoordinat QryKoordIni (int koorId)
		{
			// temukan apakah ada komponen di koordinat ini
			// ada potensi kordinat ini tidak punya komponen
			KomponenKoordinat kompIni = new KomponenKoordinat();
            try
            {
				kompIni = QryTblIni.Where(k => k.KoorId == koorId).First();
            }
            catch { }
			return kompIni;
		}
		public IQueryable<OGaris> QryGarisPola(int koorId)
		{
			// temukan apakah ada komponen di koordinat ini
			var kompIni = QryKoordIni(koorId);

			// temukan list garis yang ada di komponen terpilih
			// polaId utk komponen diambil dari tabel komponePola
			// ada potensi koordinat ini tdk punya komponen
			if(kompIni.PolaKomponen != null)
			{
				QCekBidangDatar qDatar = new QCekBidangDatar(_context, (int)kompIni.PolaKomponen.KompId);
				return qDatar.QryGarisPartial.OrderBy(g => g.Arah).AsQueryable();
            }
			//kemballikan list garis pola yang ada
			return new List<OGaris>().AsQueryable();
		}
	}
}
