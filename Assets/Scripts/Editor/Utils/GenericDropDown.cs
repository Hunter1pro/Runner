using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Powerof.Components
{
    public class GenericDropDown<T>
    {
        private TMP_Dropdown dropdown;
        private List<T> dropDownValues = new List<T>();

        public T LastValue { get; private set; }

        public GenericDropDown(TMP_Dropdown dropdown)
        {
            this.dropdown = dropdown;
        }

        public void Init(List<(T, string, Sprite)> values, Action<T> result)
        {
            this.dropdown.ClearOptions();

            values.ForEach(value =>
            {
                this.dropdown.options.Add(new TMP_Dropdown.OptionData { text = value.Item2, image = value.Item3 });
                this.dropDownValues.Add(value.Item1);
            });

            this.dropdown.onValueChanged.RemoveAllListeners();

            this.dropdown.onValueChanged.AddListener(num => { LastValue = this.dropDownValues[num]; result?.Invoke(this.dropDownValues[num]); });
        }

        public void Init(List<(T, string)> values, Action<T> result)
        {
            this.dropdown.ClearOptions();

            values.ForEach(value =>
            {
                this.dropdown.options.Add(new TMP_Dropdown.OptionData { text = value.Item2 });
                this.dropDownValues.Add(value.Item1);
            });

            this.dropdown.onValueChanged.RemoveAllListeners();

            this.dropdown.onValueChanged.AddListener(num => { LastValue = this.dropDownValues[num]; result?.Invoke(this.dropDownValues[num]); });
        }

        public void SetDefaultValue(List<(T, string, Sprite)> values, (T, string, Sprite) defaultValue)
        {
            int defaultIndex = Array.IndexOf(values.ToArray(), defaultValue);
            this.dropdown.value = defaultIndex;
            this.dropdown.options[defaultIndex].text = values[defaultIndex].Item2;
            LastValue = this.dropDownValues[defaultIndex];
        }

        public void SetDefaultValue(List<(T, string)> values, (T, string) defaultValue)
        {
            int defaultIndex = Array.IndexOf(values.ToArray(), defaultValue);
            this.dropdown.value = defaultIndex;
            this.dropdown.options[defaultIndex].text = values[defaultIndex].Item2;
            LastValue = this.dropDownValues[defaultIndex];
        }

        public void SetPosition(Vector3 position)
        {
            dropdown.transform.position = position;
        }

        public void Destroy()
        {
            GameObject.Destroy(dropdown.gameObject);
        }

        public void SetActive(bool value)
        {
            dropdown.gameObject.SetActive(value);
        }
    }
}
