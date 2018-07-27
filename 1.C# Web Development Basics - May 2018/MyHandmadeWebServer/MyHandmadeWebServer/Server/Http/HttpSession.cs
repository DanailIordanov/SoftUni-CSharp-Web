﻿namespace MyHandmadeWebServer.Server.Http
{
    using Common;
    using Contracts;

    using System;
    using System.Collections.Generic;

    public class HttpSession : IHttpSession
    {
        private readonly Dictionary<string, object> values;

        public HttpSession(string id)
        {
            CoreValidator.ThrowIfNullOrEmpty(id, nameof(id));
            
            this.Id = id;
            this.values = new Dictionary<string, object>();
        }

        public string Id { get; private set; }

        public void Add(string key, object value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNull(value, nameof(value));

            this.values[key] = value;
        }

        public void Clear() => this.values.Clear();

        public bool ContainsKey(string key) => this.values.ContainsKey(key);

        public object Get(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));

            if (!this.values.ContainsKey(key))
            {
                throw new InvalidOperationException($"The given key '{key}' is not present in the session.");
            }

            return this.values[key];
        }

        public T Get<T>(string key) => (T)this.Get(key);
    }
}