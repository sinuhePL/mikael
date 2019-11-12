﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Unit
{
    private int unitId;
    private string unitType;
    private int initialStrength;
    private int _strength;
    private int initialMorale;
    private int _morale;
    private int testModifier;
    private bool isAvialable;
    private List<Attack> unitAttacks;
    private Army owner;

    private Attack FindAttack(int aId)
    {
        Attack tempA = null;

        foreach (Attack a in unitAttacks)    // szuka ataku o wskazanym Id
        {
            if (a.GetId() == aId) tempA = a;
        }
        return tempA;
    }

    public int strength
    {
        get {return _strength;}
        set {_strength = value;}
    }
    public int morale
    {
        get { return _morale; }
        set { _morale = value; }
    }
    public string GetUnitType()
    {
        return unitType;
    }

    public Unit(Unit pattern, Army a)   // konstruktor kopiujący
    {
        unitId = pattern.unitId;
        unitType = pattern.unitType;
        initialStrength = pattern.initialStrength;
        strength = pattern.strength;
        initialMorale = pattern.initialMorale;
        morale = pattern.morale;
        testModifier = pattern.testModifier;
        isAvialable = pattern.isAvialable;
        owner = a;
        unitAttacks = new List<Attack>();
        foreach(Attack at in pattern.unitAttacks)
        {
            unitAttacks.Add(at.GetCopy(this));
        }
    }

    public Unit(int uId, string uType, int iStrength, int iMorale, Army a)  // konstruktor
    {
        unitId = uId;
        unitType = uType;
        initialStrength = iStrength;
        strength = iStrength;
        initialMorale = iMorale;
        morale = iMorale;
        testModifier = 0;           // na razie nie wiem czego będzie dotyczył
        isAvialable = true;
        unitAttacks = new List<Attack>();
        owner = a;
    }

    public StateChange MakeAttack(int aId) // wykonuje atak wskazany przez id ataku przekazane do metody
    {
        StateChange tempCS = new StateChange();
        Attack myAttack = null;

        myAttack = FindAttack(aId);
        if (myAttack != null) return myAttack.MakeAttack();   // zwraca opis zmiany stanu planszy wynikający z przeprowadzonego ataku
        else return tempCS;
    }

    public void ChangeStrength(int sc)  //zmienia siłę jednostki o wskazaną wartość
    {
        strength += sc;
    }

    public void ChangeMorale(int mc)    //zmienia morale jednostki o wskazaną wartość
    {
        morale += mc;
        owner.ChangeMorale(mc);
    }

    public List<StateChange> GetAttackOutcomes()    // zwraca wszystkie rezultaty wszystkich aktywnych ataków jednostki.
    {
        List<StateChange> tempSTList = new List<StateChange>();

        foreach(Attack a in unitAttacks)    // szuka aktywnych ataków
        {
            if(a.IsActive()) tempSTList.AddRange(a.getOutcomes());
        }
        return tempSTList;
    }

    public void ActivateAttack(int a)   // aktywuje atak o podanym Id
    {
        Attack tempAttack;

        tempAttack = FindAttack(a);
        if(tempAttack != null) tempAttack.Activate();
    }

    public void DeactivateAttack(int a)     // deaktywuje atak o podanym Id
    {
        Attack tempAttack;

        tempAttack = FindAttack(a);
        if(tempAttack != null) tempAttack.Deactivate();
    }

    public void AddAttack(Attack newAttack)  // dodaje nowy atak
    {
        unitAttacks.Add(newAttack);
    }

    public int GetArmyMorale()
    {
        return owner.GetMorale();
    }

    public int GetArmyRouteTestModifier()
    {
        return owner.GetRouteTestModifier();
    }

    public bool IsActive()
    {
        return isAvialable;
    }

    public bool ArmyTestSuccessful()
    {
        return owner.MoraleTestSuccessful();
    }

    public bool MyId(int id)    // sprawdza czy jest jednostką i podanym Id
    {
        if (id == unitId) return true;
        else return false;
    }

    public void ChangeArmyTestModifier(int modifier)
    {
        owner.ChangeRouteTestModifier(modifier);
    }

    public int GetArmyId()
    {
        return owner.GetArmyId();
    }

    public int GetUnitId()
    {
        return unitId;
    }

    public void SetAttacksTargets(List<Unit> _unitList) // ustawia referencję celów ataków na odpowiednie jednostki bazując na id celu ataku
    {
        foreach(Attack _attack in unitAttacks)
        {
            foreach(Unit _unit in _unitList)
            {
                if (_attack.CheckAndSetTarget(_unit)) break;
            }
        }
    }

    public Attack GetAttack(int idAttack)
    {
        foreach (Attack a in unitAttacks)
        {
            if (a.GetId() == idAttack) return a;
        }
        return null;
    }

    public int GetAttackOnUnit(int uId)
    {
        foreach(Attack a in unitAttacks)
        {
            if (a.GetTargetId() == uId) return a.GetId();
        }
        return 0;
    }
}
