using Microsoft.EntityFrameworkCore;
using RAB.Asset.Enum;
using RAB.Asset.Obyek;
using RAB.Data;
using RAB.Models.Utama;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Asset.OlahanModel
{
	public class AMaxSumbuId
    {
		public int X { get; set; }
		public int Y { get; set; }
    }
	public class OKetGarisKoordinat
    {
		public string Nama { get; set; }
		public OGaris Garis { get; set; }

	}
	public class QCekKoordinat
    {
		private readonly RabContext _context;
		private int _polaId;
		public QCekKoordinat(RabContext context, int polaId)
		{
			_context = context;
			_polaId = polaId;
		}
		private IQueryable<Koordinat> QryBaruRaw
		{
			get
			{
				QCekTitik qTitik = new QCekTitik(_context, _polaId);
				var qTitikIni = qTitik.QryTblIni;

				var qkoord =
					from x in qTitikIni where (int) x.Sumbu == (int) ESumbu.X
					from y in qTitikIni where (int) y.Sumbu == (int) ESumbu.Y
					select new Koordinat
					{
						TitikX = x,
						TitikY = y,
						
					};

				return qkoord;
			}
		}
		public AMaxSumbuId CekMaxSumbuId(List<Koordinat> lstKoord)
		{
			return new AMaxSumbuId
			{
				X = lstKoord.Select(x => x.TitikX.SumbuId).Max(),
				Y = lstKoord.Select(x => x.TitikY.SumbuId).Max()
			};
		}
		public IQueryable<Koordinat> BaruDariTblTitik
        {
			get
			{
				var qIni = QryBaruRaw.ToList();
				AMaxSumbuId maxXY = CekMaxSumbuId(qIni); 
				var qryBaru = from q in qIni
							select new Koordinat
							{	
								//PolaId = q.TitikX.PolaId,							
								Xid = q.TitikX.TtkId,
								Yid = q.TitikY.TtkId,
								TidakKeX = q.TitikX.SumbuId == maxXY.X || q.TitikY.SumbuId == maxXY.Y,
								TidakKeY = q.TitikX.SumbuId == maxXY.X || q.TitikY.SumbuId == maxXY.Y,
								SbgAkhirX = q.TitikX.SumbuId == maxXY.X,
								SbgAkhirY = q.TitikY.SumbuId == maxXY.Y,
								MiringAtas = false,
								MiringBawah = false,
								LengkungAtas = false,
								LengkungBawah = false,
								KiriBawahSemu = false,

								TitikX = q.TitikX,
								TitikY = q.TitikY
							};
				return qryBaru.AsQueryable();
			}
		}
		public IQueryable<OKetGarisKoordinat> GarisKoordinat	
        {
			get
			{
				var lKoor = QryTblIniPosUpToDate.ToList();
				var sumX = lKoor.GroupBy(x => x.Xid).Select(sX => sX.FirstOrDefault());
				var sumXakhir = lKoor.GroupBy(x => x.Xid).Select(sX => sX.LastOrDefault());

				var sumY = lKoor.GroupBy(x => x.Yid).Select(sX => sX.FirstOrDefault());
				var sumYakhir = lKoor.GroupBy(x => x.Yid).Select(sX => sX.LastOrDefault());

				var grsX = sumX.Select(xAw => new OKetGarisKoordinat
				{
					Nama = xAw.TitikX.Nama,
					Garis = new OGaris
					{
						Arah = ESumbu.X,
						Awal = xAw,
						Akhir = sumXakhir.First(xAk => xAk.Xid == xAw.Xid),
					}
				});
				var grsY = sumY.Select(yAw => new OKetGarisKoordinat
				{
					Nama = yAw.TitikY.Nama,
					Garis = new OGaris
					{
						Arah = ESumbu.Y,
						Awal = yAw,
						Akhir = sumYakhir.First(yAk => yAk.Yid == yAw.Yid),
					}
				});

				return grsX.Union(grsY).AsQueryable();
			}
        }
		public IQueryable<OKetGarisKoordinat> GarisKoordinat4Z
		{
			get
			{
				var sumX = QryTblIni4ZPosUpToDate.ToList();
				var maxZ = sumX.Select(z => z.TitikY.PosRel).Max();

				var awalZ = sumX.First();
				var akhirZ = sumX.Last();

				var grsX = sumX.Select(xAw => new OKetGarisKoordinat
				{
					Nama = xAw.TitikY.Nama,
					Garis = new OGaris
					{
						Arah = ESumbu.X,
						Awal = xAw,
						Akhir = new Koordinat()
						{
							TitikY = xAw.TitikY,
							TitikX = new Titik()
                            {
								PosRel = maxZ
                            }
						}
					}
				});
				var grsY = new OKetGarisKoordinat()
				{
					Nama = "Z",
					Garis = new OGaris
					{
						Arah = ESumbu.Y,
						Awal = awalZ,
						Akhir = akhirZ,
					}
				};

				return grsX.Append(grsY).AsQueryable();
			}
		}
		public IQueryable<Koordinat> QryTblIni
		{
			get{
				var qIni = _context.TblKoordinat.Include(k => k.TitikX).Include(k => k.TitikY)
							.Where(t => t.TitikX.PolaId == _polaId)
							.Where(t => t.TitikY.PolaId == _polaId)
							.OrderBy(o => o.Yid).ThenBy(o2 => o2.Xid)
							.ToList();
				return qIni.AsQueryable();
			}
		}
		public IQueryable<Koordinat> QryTblIniPosUpToDate
        {
            get
            {
				QCekTitik qTitik = new QCekTitik(_context, _polaId);
				var qTitikIni = qTitik.QryIniBerposisi.ToList();

				var koor = from k in QryTblIni
						   join tX in qTitikIni on k.TitikX.TtkId equals tX.TtkId
						   join ty in qTitikIni on k.TitikY.TtkId equals ty.TtkId
						   select new Koordinat()
						   {
							   KoordId = k.KoordId,
							   TitikX = tX,
							   TitikY = ty,
							   TidakKeX = k.TidakKeX,
							   TidakKeY = k.TidakKeY,

							   SbgAwalX = k.SbgAwalX,
							   SbgAwalY = k.SbgAwalY,

							   SbgAkhirX = k.SbgAkhirX,
							   SbgAkhirY = k.SbgAkhirY,
							   
							   MiringAtas = k.MiringAtas,
							   MiringBawah = k.MiringBawah,
							   
							   LengkungAtas = k.LengkungAtas,
							   LengkungBawah = k.LengkungBawah,
							   KiriBawahSemu = k.KiriBawahSemu,
							   Xid = k.Xid,
							   Yid = k.Yid
						   };
				return koor;
            }
        }
		public void UpdateDariTblTitik()
		{
			//QCekKoordinat qKoord = new QCekKoordinat(_context, _polaId);
			var qBaruDariTblTitik = BaruDariTblTitik.ToList();
			// hapus jika namanya tidak ada di TblGrid

			var lstTblIni = QryTblIni.ToList();
			var qLamaUntukDihapus = lstTblIni.Where(l => !qBaruDariTblTitik.Select(g => g.Nama).Contains(l.Nama)).ToList();

			// ditambahkan, hanya koodinat baru dari TblGrid yang namanya tidak ada di TblTtkPotong
			var qBaruUntukDitambahkan = qBaruDariTblTitik.Where(b => !lstTblIni.Select(h => h.Nama).Contains(b.Nama)).ToList();

			try{
				if (qLamaUntukDihapus.Count > 0){
					_context.TblKoordinat.RemoveRange(qLamaUntukDihapus);
					_context.SaveChanges();
				}
			}
			catch (Exception) { }

			try{
				if (qBaruUntukDitambahkan.Count > 0){
					_context.TblKoordinat.AddRange(qBaruUntukDitambahkan);
					_context.SaveChanges();
				}
			}
			catch (Exception) { }
		}

		public IQueryable<Koordinat> QryTblIni4ZPosUpToDate
		{
			get
			{
				QCekTitik qTitik = new QCekTitik(_context, _polaId);
				var qTitikIni = qTitik.QryIni4ZBerposisi.ToList();
				var sumXawal = qTitik.QryIniBerposisi.Where(t => (int)t.Sumbu == (int)ESumbu.X).First();
				var koor = from k in qTitikIni
						   select new Koordinat()
						   {
							   TitikX = sumXawal,
							   TitikY = k,
							   Xid = sumXawal.TtkId,
							   Yid = k.TtkId
						   };
				return koor.AsQueryable();
			}
		}

	}
}
