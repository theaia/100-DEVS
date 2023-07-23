using System.Collections;
using UnityEngine;

public class IDamageable : MonoBehaviour {
	
	private int currentHealth;
	[SerializeField] private int maxHealth;
	[SerializeField] Renderer rend;
	[SerializeField] float punchFXStrength;
	[SerializeField] float flashDuration;
	[SerializeField] string shaderPropertyForFlash = "";
	[SerializeField] Color finalColorOnHealthDecrease;
	[SerializeField] private float renableAfterSeconds;
	private Color[] startingMatColors;
	private Vector3 startingScale;
	
	private void Awake() {
		startingScale = rend.gameObject.transform.localScale;
		
		if (rend) {
			startingMatColors = new Color[rend.materials.Length];
			for (int i = 0; i < startingMatColors.Length; i++) {
				if (shaderPropertyForFlash != "") {
					startingMatColors[i] = rend.materials[i].GetColor(shaderPropertyForFlash);
				} else {
					startingMatColors[i] = rend.materials[i].color;
				}
			}
		}
	}

	private void OnEnable() {
		RestoreFullHealth();
		transform.localScale = Vector3.one;
	}

	public void RestoreFullHealth() {
		SetHealth(maxHealth);
	}
	
	public void Damage(int amount) {
		currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
		if (rend) {
			StartCoroutine(Flash(rend, startingMatColors, Color.white * 2f, flashDuration, shaderPropertyForFlash));
		}

		if (currentHealth <= 0) {
			if (renableAfterSeconds > 0) {
				StartCoroutine(DisableForSeconds(renableAfterSeconds));
			} else {
				Destroy(gameObject);
			}
		}
	}

	IEnumerator DisableForSeconds(float _seconds) {
		rend.gameObject.SetActive(false);
		yield return new WaitForSeconds(_seconds);
		RestoreFullHealth();
		rend.gameObject.SetActive(true);
	}
	

	public void SetHealth(int _value) {
		currentHealth = _value;
	}

	private IEnumerator Flash(Renderer _rend, Color[] _startingColors, Color _targetColor, float _duration, string customShaderProperty = "") {
		float _elapsedTime = 0f;
		float _healthPct = currentHealth / maxHealth;
		while (_elapsedTime < _duration) {
			if (_elapsedTime < (_duration / 2)) { //Flash to hit color for 1/2 duration
				for (int i = 0; i < _rend.materials.Length; i++) {
					Color _modifiedColor = Color.Lerp(_startingColors[i], _targetColor, _elapsedTime / _duration);
					if (customShaderProperty != "") {

						_rend.materials[i].SetColor(customShaderProperty, _modifiedColor);
					} else {
						_rend.materials[i].color = _modifiedColor;
					}
				}
				rend.gameObject.transform.localScale = Vector3.Lerp(startingScale, startingScale * punchFXStrength, _elapsedTime / _duration);
			
			} else { //Flash to back to color for second half

				for (int i = 0; i < _rend.materials.Length; i++) {
					Color _startingColor = _startingColors[i];
					Color _modifiedColor = Color.Lerp(_targetColor, _startingColor, _elapsedTime / _duration);

					if (customShaderProperty != "") {
						_rend.materials[i].SetColor(customShaderProperty, _modifiedColor);
					} else {
						_rend.materials[i].color = _modifiedColor;
					}
				}
				rend.gameObject.transform.localScale = Vector3.Lerp(startingScale * punchFXStrength, startingScale, _elapsedTime / _duration);
			}
			_elapsedTime += Time.deltaTime;
			yield return null;
		}

		//Set final color
		for (int i = 0; i < _rend.materials.Length; i++) {
			Color _startingColor = startingMatColors[i];
			if (customShaderProperty != "") {
				_rend.materials[i].SetColor(customShaderProperty, _startingColor);
			} else {
				_rend.materials[i].color = _startingColor;
			}
		}

		rend.gameObject.transform.localScale = startingScale;
	}
}
