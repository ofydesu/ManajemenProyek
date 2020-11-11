using Microsoft.EntityFrameworkCore;
using RAB.Asset.Enum;
using RAB.Asset.Obyek;
using RAB.Asset.OlahanModel;
using RAB.Data;
using RAB.Models.Utama;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.BisnisModel.OlahanModel
{
	public class AListGarisPola
	{
		public Pola Pola { get; set; }
        public List<OGaris> LstGaris { get; set; }
    }

	public class QCekKompGaris
    {
		private readonly RabContext _context;
		private int _polaId;
		private int _garisId;
		public QCekKompGaris(RabContext context, int polaId)
		{
			_context = context;
			_polaId = polaId;
		}
		public QCekKompGaris(RabContext context, int polaId, int garisId)
		{
			_context = context;
			_polaId = polaId;
			_garisId = garisId;
		}
		public IQueryable<KomponenGaris> QryTblIni
		{
			get
			{
				var qIni = _context.TblKomponenGaris
							.Include(k=>k.Garis)
								.Include(k => k.Garis.KoordAwal)
									.Include(k => k.Garis.KoordAwal.TitikX)
										.Include(k => k.Garis.KoordAwal.TitikX.Pola)
									.Include(k => k.Garis.KoordAwal.TitikY)
										.Include(k => k.Garis.KoordAwal.TitikY.Pola)
								.Include(k => k.Garis.KoordAkhir)
									.Include(k => k.Garis.KoordAkhir.TitikX)
										.Include(k => k.Garis.KoordAkhir.TitikX.Pola)
									.Include(k => k.Garis.KoordAkhir.TitikY)
										.Include(k => k.Garis.KoordAkhir.TitikY.Pola)
							.Include(k => k.PolaKomponen)
								.Include(k => k.PolaKomponen.Pola)
								.Include(k => k.PolaKomponen.Komponen)
							.Where(k=>k.Garis.KoordAwal.TitikX.PolaId == _polaId)
							;
				return qIni;
			}
		}

		public IQueryable<KomponenGaris>QryTblIniByGaris
        {
			get
			{
				return QryTblIni.Where(k => k.Garis.GarisId == _garisId);
			}
        }
		public List<OGaris> CekListGarisPola(int polaId)
        {
			QCekBidangDatar qDatar = new QCekBidangDatar(_context, polaId);
			var qGaris = qDatar.QryGarisPartial.OrderBy(g => g.Arah);
			return qGaris.ToList();
        }
	public IQueryable<OGaris> QryGarisPolaRelatif
        {
            get
            {
				var lstKomp = QryTblIniByGaris;
				var polaBalok = lstKomp.Where(k => (int)k.PolaKomponen.Komponen.Posisi3D == (int)EPosisi3D.Balok)
									.Select(k => k.PolaKomponen.Komponen);

				var kordPolaBalok = polaBalok.Select(b => new AListGarisPola()
				{
					Pola = b,
					LstGaris = CekListGarisPola(b.PolaId)
				});

				// nanti dilajutkan!!!
				return null;
            }
        }
	}
}
