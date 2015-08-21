using System;
using System.Collections.Generic;
using System.Text;

namespace TouchRemote
{
  public enum  ButtonType {  Keystroke }

  public class TouchMenuOptions
  {
    public const int MaxButtons = 8;
    public const int MinButtons = 1;
    private Dictionary<string, TouchButtonSet> _sets;
    private string _activeSet;

    public Dictionary<string, TouchButtonSet> Sets { get { return _sets; } }
    public int Count { get { return _sets.Count; } }

    public void Add(TouchButtonSet NewButtonSet)
    {
      _sets[NewButtonSet.Name] = NewButtonSet;
    }

    public string SelectedName {
      get { return _activeSet; }
      set { if (_sets.ContainsKey(value)) _activeSet = value; }
    }

    public TouchButtonSet SelectedSet
    {
      get { return _sets[_activeSet]; }
      set
      {
        if (_sets.ContainsKey(value.Name))
          _activeSet = value.Name;
      }
    }

    public TouchMenuOptions()
    {
      _sets = new Dictionary<string, TouchButtonSet>();
      TouchButtonSet bs = new TouchButtonSet("CCP");
      bs.Add("Cut", "^x");
      bs.Add("Copy", "^c");
      bs.Add("Paste", "^v");
      bs.Add("Home", "{Home}");
      bs.Add("End", "{End}");
      bs.Add("SA", "^a");
      this.Add(bs);
      SelectedName = bs.Name;

      bs = new TouchButtonSet("Feedly");
      bs.Add("Next", "j");
      bs.Add("Prev", "k");
      bs.Add("Ref", "r");
      this.Add(bs);
      SelectedName = bs.Name;
    }

    public class TouchButtonSet
    {
      private string _name;
      private Dictionary<string,TouchButton> _buttons;

      public TouchButton this[string ButtonName]
      {
        get
        {
          return _buttons[ButtonName];
        }
      }

      public string Name { get { return _name; } }

      public int ButtonCount {  get { return _buttons.Count; } }

      public Dictionary<string,TouchButton> Buttons { get { return _buttons; } }

      public void Add(string Name, string Keys)
      {
        _buttons.Add(Name, new TouchButton(Name, Keys));
      }

      public void Add(TouchButton NewButton)
      {
        _buttons.Add(NewButton.Name, NewButton);
      }

      public TouchButtonSet(string Name)
      {
        _name = Name;
        _buttons = new Dictionary<string, TouchButton>();
      }
    }

    public class TouchButton
    {
      private ButtonType _type;
      private string _name;
      private string _keys;

      public TouchButton(ButtonType Type)
      {
        _type = Type;
        _name = _type.ToString();
      }

      public TouchButton(string Name, string Keys)
      {
        _type = ButtonType.Keystroke;
        _name = Name;
        _keys = Keys;
      }

      public string Name { get { return _name; } }
      public ButtonType Type { get { return _type; } }
      public string Keys { get { return _keys; } }
    }
  }
}
