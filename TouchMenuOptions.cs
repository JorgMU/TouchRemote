using System;
using System.Collections.Generic;
using System.Text;

namespace TouchRemote
{
  public enum  ButtonType {  Cut, Copy, Paste, Keystroke }

  public class TouchMenuOptions
  {
    public const int MaxButtons = 8;
    public const int MinButtons = 1;
    private Dictionary<string, TouchButtonSet> _sets;
    private string _activeSet;

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
      bs.Add(new TouchButton(ButtonType.Cut));
      bs.Add(new TouchButton(ButtonType.Copy));
      bs.Add(new TouchButton(ButtonType.Paste));
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

      public TouchButton(ButtonType Type)
      {
        _type = Type;
        _name = _type.ToString();
      }

      public string Name { get { return _name; } }
      public ButtonType Type { get { return _type; } }
    }
  }
}
