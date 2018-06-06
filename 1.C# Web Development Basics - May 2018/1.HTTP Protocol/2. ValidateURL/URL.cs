using System;
using System.Text;

namespace ValidateURL
{
    public class URL
    {
        private string protocol;
        private string host;
        private string port;
        private string path;
        private string queryStrings;
        private string fragment;

        public URL(string protocol, string host, string port, string path)
        {
            this.Protocol = protocol;
            this.Host = host;
            this.Port = port;
            this.Path = path;
        }

        public URL(string protocol, string host, string port, string path, string queryStrings)
            : this(protocol, host, port, path)
        {
            this.QueryStrings = queryStrings;
        }

        public URL(string protocol, string host, string port, string path, string queryStrings, string fragment)
            : this(protocol, host, port, path, queryStrings)
        {
            this.Fragment = fragment;
        }

        public string Protocol
        {
            get
            {
                return this.protocol;
            }
            set
            {
                this.protocol = value;
            }
        }

        public string Host
        {
            get
            {
                return this.host;
            }
            set
            {
                this.host = value;
            }
        }

        public string Port
        {
            get
            {
                return this.port;
            }
            set
            {
                if (value == "")
                {
                    if (this.protocol == "http")
                    {
                        this.port = "80";
                        return;
                    }
                    else
                    {
                        this.port = "443";
                        return;
                    }
                }
                else
                {
                    if (this.protocol == "http" && value == "443")
                    {
                        throw new ArgumentException();
                    }
                    if (this.protocol == "https" && value == "80")
                    {
                        throw new ArgumentException();
                    }
                }
                this.port = value;
            }
        }

        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                if (value == "")
                {
                    this.path = "/";
                    return;
                }
                this.path = value;
            }
        }

        public string QueryStrings
        {
            get
            {
                return this.queryStrings;
            }
            set
            {
                this.queryStrings = value;
            }
        }

        public string Fragment
        {
            get
            {
                return this.fragment;
            }
            set
            {
                this.fragment = value;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            var properties = this.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var propValue = prop.GetValue(this, null).ToString();
                if (propValue == "")
                {
                    continue;
                }

                sb.AppendLine($"{prop.Name}: {propValue}");
            }

            return sb.ToString();
        }
    }
}
