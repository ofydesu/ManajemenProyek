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
		public int GarisId { get; set; }
		public int KompId { get; set; }
        public List<OGaris> LstGaris { get; set; }
    }

	public class AKomplexPolaPersegi
    {
		public AGaris DatarBawah { get; set; }
		public AGaris DatarAtas { get; set; }
		public AGaris TegakKiri { get; set; }
		public AGaris TegakKanan { get;set; }
    }
	public class AKomplexGaris
    {
		public AKomplexPolaPersegi KompAwal { get; set; }
        public AGaris GarisIni { get; set; }
		public AKomplexPolaPersegi KompAkhir { get; set; }
	}

	public class QCekKompGaris
    {
		private readonly RabContext _context;
		private int _polaId;
		private int _garisId;
		private AKomplexGaris _garisUpToDate;
		//public QCekKompGaris(RabContext context, int polaId)
		//{
		//	_context = context;
		//	_polaId = polaId;
		//}
		public QCekKompGaris(RabContext context, int polaId, int garisId)
		{
			_context = context;
			_polaId = polaId;
			_garisId = garisId;
		}

		public IQueryable<AGaris> GarisKomponen
        {
            get
            {
				//membuat garis di koordinat
				_garisUpToDate = KomplexGarisUpToDate;

				var koleksi = QryGarisPolaKolom.Union(QryGarisPolaBalok);
				return koleksi;
            }
        }
		public IQueryable<KomponenGaris> QryTblIniByGaris
		{
			get
			{
				var qIni = _context.TblKomponenGaris
							//.Include(k=>k.Garis)
							//	.Include(k => k.Garis.KoordAwal)
							//		.Include(k => k.Garis.KoordAwal.TitikX)
							//			.Include(k => k.Garis.KoordAwal.TitikX.Pola)
							//		.Include(k => k.Garis.KoordAwal.TitikY)
							//			.Include(k => k.Garis.KoordAwal.TitikY.Pola)
							//	.Include(k => k.Garis.KoordAkhir)
							//		.Include(k => k.Garis.KoordAkhir.TitikX)
							//			.Include(k => k.Garis.KoordAkhir.TitikX.Pola)
							//		.Include(k => k.Garis.KoordAkhir.TitikY)
							//			.Include(k => k.Garis.KoordAkhir.TitikY.Pola)
							//.Include(k => k.PolaKomponen)
							//	.Include(k => k.PolaKomponen.Pola)
							//	.Include(k => k.PolaKomponen.Komponen)
							//.Where(k=>k.Garis.KoordAwal.TitikX.PolaId == _polaId)
							.Where(k => k.Garis.GarisId == _garisId)
							;
				return qIni;
			}
		}
		private List<OGaris> CekListGarisPola(int polaId)
        {
			QCekBidangDatar qDatar = new QCekBidangDatar(_context, polaId);
			var qGaris = qDatar.QryGarisPartial.OrderBy(g => g.Arah);
			return qGaris.ToList();
        }
		private IQueryable<AGaris> QryGarisPolaKolom
		{
            get
            {
				List<AGaris> lstGrs = new List<AGaris>();
				lstGrs.Add(_garisUpToDate.KompAwal.DatarAtas);
				lstGrs.Add(_garisUpToDate.KompAwal.DatarBawah);
				lstGrs.Add(_garisUpToDate.KompAwal.TegakKiri);
				lstGrs.Add(_garisUpToDate.KompAwal.TegakKanan);

				lstGrs.Add(_garisUpToDate.KompAkhir.DatarAtas);
				lstGrs.Add(_garisUpToDate.KompAkhir.DatarBawah);
				lstGrs.Add(_garisUpToDate.KompAkhir.TegakKiri);
				lstGrs.Add(_garisUpToDate.KompAkhir.TegakKanan);

				// filter untuk garis yang bernilai null
				var lstGrsTerisi = lstGrs.Where(k => k != null);
				
				return lstGrsTerisi.AsQueryable();

            }
		}

		private IQueryable<AGaris> QryGarisPolaBalok
        {
            get
            {
				var garisIniUpToDate = _garisUpToDate.GarisIni;
				var polaBalok = QryTblIniByGaris.Where(k => (int)k.PolaKomponen.Komponen.Posisi3D == (int)EPosisi3D.Balok);

				var kordPolaBalok = polaBalok.Select(b => new AListGarisPola()
				{
					GarisId = b.GarisId,
					KompId = b.PolaKomponen.Komponen.PolaId,
					LstGaris = CekListGarisPola(b.PolaKomponen.Komponen.PolaId)
				});

				var posAtas = polaBalok.Where(k => k.PosRelatifDiZ == EPosKompDiZ.Atas);
				List<AGaris> lstGarisBalok = new List<AGaris>();
				foreach (var pos in posAtas)
                {
					var elAcuan = pos.TitikZ.PosRel;
					var tegakKiri = new AGaris
					{
						KoorAkhir = new AKoord
						{
							Nama = "kiAtas",
							PosX = garisIniUpToDate.KoorAwal.PosX,   // sesuai garis uptodate
							PosY = elAcuan
						},
						KoorAwal = new AKoord
						{
							Nama = "kiBawah",
							PosX = _garisUpToDate.GarisIni.KoorAwal.PosX, // sesuai garis upToDate
							PosY = elAcuan - pos.Garis.Panjang
						}


					};
					var tegakKanan = new AGaris
					{
						KoorAkhir = new AKoord
						{
							Nama = "kaAtas",
							PosX = garisIniUpToDate.KoorAkhir.PosX,   // sesuai garis uptodate
							PosY = elAcuan
						},
						KoorAwal = new AKoord
						{
							Nama = "kaBawah",
							PosX = garisIniUpToDate.KoorAkhir.PosX, // sesuai garis upToDate
							PosY = elAcuan - pos.Garis.Panjang
						}


					};
					var datarBawah = new AGaris
					{
						KoorAkhir = new AKoord
						{
							Nama = "kiBawah",
							PosX = garisIniUpToDate.KoorAwal.PosX,   // sesuai garis uptodate
							PosY = elAcuan - pos.Garis.Panjang
						},
						KoorAwal = new AKoord
						{
							Nama = "kaBawah",
							PosX = garisIniUpToDate.KoorAkhir.PosX, // sesuai garis upToDate
							PosY = elAcuan - pos.Garis.Panjang
						}


					};
					var datarAtas = new AGaris
					{
						KoorAkhir = new AKoord
						{
							Nama = "kiAtas",
							PosX = garisIniUpToDate.KoorAwal.PosX,   // sesuai garis uptodate
							PosY = elAcuan
						},
						KoorAwal = new AKoord
						{
							Nama = "kaAtas",
							PosX = garisIniUpToDate.KoorAkhir.PosX, // sesuai garis upToDate
							PosY = elAcuan
						}


					};
					lstGarisBalok.Add(tegakKiri);
					lstGarisBalok.Add(tegakKanan);
					lstGarisBalok.Add(datarAtas);
					lstGarisBalok.Add(datarAtas);
				};

				return lstGarisBalok.AsQueryable();
            }
        }

		private AKomplexGaris KomplexGarisUpToDate
        {
            get
            {
                #region pengumpulan data
                // dapatkan ukuran asli garis terpilih
                var qcGaris = new QCekGaris(_context, _polaId);
				var grsAsal = qcGaris.QryTblIniPosUpToDate.Where(g => g.GarisId == _garisId).First();
				//-----------------------------------------------
				// cek apakah ada kolom di koordinat awal dan akhir di garis terpilih-------------------------
				var qcKompKoor = new QCekKompKoordinat(_context, _polaId);
				var qkompAwal = qcKompKoor.QryGarisPola(grsAsal.KoordAwal.KoordId);
				var qkompAkhir = qcKompKoor.QryGarisPola(grsAsal.KoordAkhir.KoordId);
				// posisi z terbaru
				var qcKoor = new QCekKoordinat(_context, _polaId);
				var qcKoorZ = qcKoor.QryTblIni4ZPosUpToDate;
                //--------------------------------------------------------------------------
                #region internal
                // buat objek baru utk garis ini pada kondisi asli garis
                // agar tetap sama jika tidak ada komponen di koordinatnya
                var garisIni = new AGaris()
				{
					KoorAwal = new AKoord()
					{
						PosX = grsAsal.KoordAwal.TitikX.PosRel,
						PosY = 0
					},
					KoorAkhir = new AKoord()
					{
						PosX = grsAsal.KoordAkhir.TitikX.PosRel,
						PosY = 0
					}
				};
				//---------------------------------------
				//buat objek baru untuk komplex garis terpilih-----
				var komplGaris = new AKomplexGaris() {
					GarisIni = garisIni,
					KompAwal = new AKomplexPolaPersegi(),
					KompAkhir = new AKomplexPolaPersegi()
				};
				//------------------------------------------------------
				//objek baru untuk garis X dan Y---
				var grsXKomp = new OGaris();
				var grsYKomp = new OGaris();
				//--------------------------------------
				//variabel baru untuk posisi acuan komponen koordinat
				int posYatas = 0;
				int posYbawah = 0;
                //---------------------------------------------------
                #endregion
                #endregion
                //jika garis memiliki komponen di koordinat awal
                if (qkompAwal != null && qkompAwal.Any())
                {
                    #region pencarian data
                    // ambil property untuk komponen awal
                    var qcKoorAwal = qcKompKoor.QryKoordIni(grsAsal.KoordAwal.KoordId);
					// ambil nilai acuan atas
					// ambil daari koord Z uptodate yang kodenya sesuai dengan kode koord awal

					posYatas = qcKoorZ.Where(z=>z.TitikY.TtkId == qcKoorAwal.TitikZatas.TtkId).Single().TitikY.PosRel;
					posYbawah = qcKoorZ.Where(z => z.TitikY.TtkId == qcKoorAwal.TitikZbawah.TtkId).Single().TitikY.PosRel;
					//---------------------------------------------------
					//buat penyesuaian terhadap garis pola
					if (grsAsal.Arah == ESumbu.X)
						grsXKomp = qkompAwal.Where(k => k.Arah == ESumbu.X).First();
					else
						grsXKomp = qkompAwal.Where(k => k.Arah == ESumbu.Y).First();
					//----------------------------------------------------------------
					// karena komponen harus berpusat di koordinat,
					// maka harus di-offset ke kanan/kiri 
					int offsetKeKanan = grsXKomp.Panjang / 2;
					//----------------------------------------------
					// geser garis asal sejauh offset ke arah kanan
					komplGaris.GarisIni.KoorAwal.PosX = offsetKeKanan;
					komplGaris.GarisIni.KoorAwal.PosY = 0;
                    //----------------------------------------------------
#endregion
                    // buat komplex pola persegi panjang untuk komponen ini
                    var kompAwal = new AKomplexPolaPersegi()
					{
						DatarBawah = new AGaris()
						{
							KoorAwal = new AKoord()
							{
								PosX = -offsetKeKanan,
								PosY = posYbawah // tergantung pada acuan
							},
							KoorAkhir = new AKoord()
							{
								PosX = offsetKeKanan,
								PosY = posYbawah // tergantung pada acuan
							},
						},
						DatarAtas = new AGaris()
						{
							KoorAwal = new AKoord()
							{
								PosX = -offsetKeKanan,
								PosY = posYatas // tergantung pada acuan
							},
							KoorAkhir = new AKoord()
							{
								PosX = offsetKeKanan,
								PosY = posYatas // tergantung pada acuan
							}
						},
						TegakKiri = new AGaris()
						{
							KoorAwal = new AKoord()
							{
								PosX = -offsetKeKanan,
								PosY = posYbawah // tergantung pada acuan
							},
							KoorAkhir = new AKoord()
							{
								PosX = -offsetKeKanan,
								PosY = posYatas // tergantung pada acuan
							}
						},
						TegakKanan = new AGaris()
						{
							KoorAwal = new AKoord()
							{
								PosX = offsetKeKanan,
								PosY = posYbawah // tergantung pada acuan
							},
							KoorAkhir = new AKoord()
							{
								PosX = offsetKeKanan,
								PosY = posYatas // tergantung pada acuan
							}
						},
					};
					//masukkan nilai komplex awal
					komplGaris.KompAwal = kompAwal;
				}
				//jika garis memiliki komponen di koordinat akhir
				if (qkompAkhir != null && qkompAkhir.Any())
				{
                    #region pencarian data
                    // ambil property untuk komponen akhir
                    var qcKoorAkhir = qcKompKoor.QryKoordIni(grsAsal.KoordAkhir.KoordId);
					// ambil nilai acuan atas
					// ambil daari koord Z uptodate yang kodenya sesuai dengan kode koord awal
					posYatas = qcKoorZ.Where(z => z.TitikY.TtkId == qcKoorAkhir.TitikZatas.TtkId).Single().TitikY.PosRel;
					posYbawah = qcKoorZ.Where(z => z.TitikY.TtkId == qcKoorAkhir.TitikZbawah.TtkId).Single().TitikY.PosRel;
					//---------------------------------------------------
					if (grsAsal.Arah == ESumbu.X)
						grsXKomp = qkompAkhir.Where(k => k.Arah == ESumbu.X).First();
					else
						grsXKomp = qkompAkhir.Where(k => k.Arah == ESumbu.Y).First();
					//--------------------------------------
					// temukan offset ke kanan
					var offsetKeKanan = grsXKomp.Panjang / 2;
					//------------------------------------------
					// temukan posisi ujung kanan garis asal setelah offset
					var akhirGrsAsal = grsAsal.Panjang - offsetKeKanan;
					var posUjung = akhirGrsAsal + grsXKomp.Panjang;
					//---------------------------------------------------------------------
					// geser garis asal sejauh offset ke arah kiri
					komplGaris.GarisIni.KoorAkhir.PosX = akhirGrsAsal;
					komplGaris.GarisIni.KoorAkhir.PosY = 0;
					//----------------------------------------------------
					#endregion
					// masukkan isian koordinat komp akhir 
					var kompAkhir = new AKomplexPolaPersegi()
					{
						DatarBawah = new AGaris()
						{
							KoorAwal = new AKoord()
							{
								PosX = akhirGrsAsal,
								PosY = posYbawah // tergantung pada acuan
							},
							KoorAkhir = new AKoord()
							{
								PosX = akhirGrsAsal + (2 * offsetKeKanan),
								PosY = posYbawah // tergantung pada acuan
							},
						},
						DatarAtas = new AGaris()
						{
							KoorAwal = new AKoord()
							{
								PosX = akhirGrsAsal,
								PosY = posYatas // tergantung pada acuan
							},
							KoorAkhir = new AKoord()
							{
								PosX = posUjung,
								PosY = posYatas // tergantung pada acuan
							}
						},
						TegakKiri = new AGaris()
						{
							KoorAwal = new AKoord()
							{
								PosX = akhirGrsAsal,
								PosY = posYbawah // tergantung pada acuan
							},
							KoorAkhir = new AKoord()
							{
								PosX = akhirGrsAsal,
								PosY = posYatas // tergantung pada acuan
							}
						},
						TegakKanan = new AGaris()
						{
							KoorAwal = new AKoord()
							{
								PosX = posUjung,
								PosY = posYbawah // tergantung pada acuan
							},
							KoorAkhir = new AKoord()
							{
								PosX = posUjung,
								PosY = posYatas // tergantung pada acuan
							}
						},
					};
					// simpan nila kimp akhir
					komplGaris.KompAkhir = kompAkhir;
				}
				return komplGaris;
			}
		}

	}
}
