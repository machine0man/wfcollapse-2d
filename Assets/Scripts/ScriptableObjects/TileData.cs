using System.Collections.Generic;
using UnityEngine;

namespace Nature
{
	[CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObjects/TileData", order = 1)]
	public class TileData : ScriptableObject
	{
		[SerializeField] int m_uId;
		[SerializeField] Sprite m_spriteTile;
		[SerializeField] List<int> m_lstTop;
		[SerializeField] List<int> m_lstLeft;
		[SerializeField] List<int> m_lstRight;
		[SerializeField] List<int> m_lstBottom;

		public int UId => m_uId;
		public Sprite SpriteTile => m_spriteTile;
		public IReadOnlyList<int> LstTop => m_lstTop.AsReadOnly();
		public IReadOnlyList<int> LstLeft => m_lstLeft.AsReadOnly();
		public IReadOnlyList<int> LstRight => m_lstRight.AsReadOnly();
		public IReadOnlyList<int> LstBottom => m_lstBottom.AsReadOnly();
	}
}
