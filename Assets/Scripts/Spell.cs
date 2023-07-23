using UnityEngine;

[CreateAssetMenu(menuName = "Spell")]
public class Spell : ScriptableObject {
	public string spellName;
	public Sprite spellIcon;
	public SpellType spellType;
	public GameObject spellPrefab;
	public float spellCooldown;
	public float spellChargeTime;
	public ParticleSystem spellChargeParticles;
	public ParticleSystem spellChargeFinishedParticles;
}

public enum SpellType {
	Projectile,
	Charge
}
