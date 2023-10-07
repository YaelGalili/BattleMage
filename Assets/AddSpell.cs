using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpell : MonoBehaviour
{
    [SerializeField] SpellCaster.Spell spell;

    
    public void OnClick() {
        SpellCaster.AddSpell(spell);
    }
}
