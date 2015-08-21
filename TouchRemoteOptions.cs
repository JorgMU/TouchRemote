using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace TouchRemote
{
  public enum  ButtonType {  Keystroke }

  public class TouchRemoteOptions
  {
    private string _activeSet;
    private Dictionary<string, TouchButtonSet> _sets;

    public Dictionary<string, TouchButtonSet> Sets { get { return _sets; } }

    public TouchRemoteOptions()
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

      _activeSet = bs.Name;
    }

    public int SetCount { get { return _sets.Count; } }

    public void Add(TouchButtonSet NewButtonSet)
    {
      _sets[NewButtonSet.Name] = NewButtonSet;
    }

    public void Remove(string SetName)
    {
      if (_sets.ContainsKey(SetName))
        _sets.Remove(SetName);

      if (_sets.Count < 1) _activeSet = "";
      else if (!_sets.ContainsKey(_activeSet))
        foreach(string s in _sets.Keys)
        {
          _activeSet = s;
          break;
        }
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

    public string Select(string Name)
    {
      if (_sets.ContainsKey(Name)) _activeSet = Name;
      return _activeSet;
    }

    public void Save(FileInfo OptionsFile)
    {
      StreamWriter sw = null;
      try
      {
        sw = new StreamWriter(OptionsFile.OpenWrite());

        sw.WriteLine("active=" + _activeSet);

        foreach (TouchButtonSet s in _sets.Values)
          foreach (TouchButton b in s.Buttons.Values)
            sw.WriteLine("button={0}|{1}|{2}", s.Name, b.Name, b.Keys);
      }
      catch (SystemException se) { ShowError(se); }
      finally { if (sw != null) sw.Close(); }
    }

    public void Load(FileInfo OptionsFile)
    {
      if (!OptionsFile.Exists) return;

      StreamReader sr = null;
      try {
        sr = OptionsFile.OpenText();
        while(!sr.EndOfStream)
        {
          string line = sr.ReadLine();
          string[] parts = line.Split("=".ToCharArray(), 2);
          if (parts.Length < 2) continue;
          switch (parts[0])
          {
            case "button":
              string[] subparts = parts[1].Split("|".ToCharArray(), 3);
              if (subparts.Length < 3) continue;
              string s = subparts[0];
              string bn = subparts[1];
              string bk = subparts[2];
              if (!Sets.ContainsKey(s))
                _sets.Add(s, new TouchButtonSet(s));
              _sets[s].Add(bn, bk);
              break;
            case "active":
              _activeSet = parts[1];
              break;
            default:
              break;
          }
        }
        if(!_sets.ContainsKey(_activeSet))
          foreach(string s in _sets.Keys)
          {
            _activeSet = s;
            break;
          }
      }
      catch (SystemException se) { ShowError(se); }
      finally { if (sr != null) sr.Close(); }
    }

    private void ShowError(SystemException se)
    {
      string msg = se.Message;
      if (se.InnerException != null) msg += "\r\n" + se.InnerException.Message;
      MessageBox.Show(msg, "TouchRemoteOptions", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        _buttons[Name] = new TouchButton(Name, Keys);
      }

      public void Remove(string ButtonName)
      {
        if (_buttons.ContainsKey(ButtonName))
          _buttons.Remove(ButtonName);
      }

      public TouchButtonSet(string Name)
      {
        _name = Name;
        _buttons = new Dictionary<string, TouchButton>();
      }
    }

    public class TouchButton
    {
      private string _name;

      private string _keys;

      public TouchButton(string Name, string Keys)
      {
        _name = Name;
        _keys = Keys;
      }

      public string Name { get { return _name; } }

      public string Keys { get { return _keys; } }
    }
  }
}
