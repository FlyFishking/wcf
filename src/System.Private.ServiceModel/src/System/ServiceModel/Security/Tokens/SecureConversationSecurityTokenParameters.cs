// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.ServiceModel.Channels;
using System.Text;

namespace System.ServiceModel.Security.Tokens
{
    public class SecureConversationSecurityTokenParameters : SecurityTokenParameters
    {
        internal const bool defaultRequireCancellation = true;
        internal const bool defaultCanRenewSession = true;

        private SecurityBindingElement _bootstrapSecurityBindingElement;
        private bool _requireCancellation;
        private bool _canRenewSession = defaultCanRenewSession;
        private BindingContext _issuerBindingContext;

        protected SecureConversationSecurityTokenParameters(SecureConversationSecurityTokenParameters other)
            : base(other)
        {
            _requireCancellation = other._requireCancellation;
            _canRenewSession = other._canRenewSession;
            if (other._bootstrapSecurityBindingElement != null)
                _bootstrapSecurityBindingElement = (SecurityBindingElement)other._bootstrapSecurityBindingElement.Clone();

            if (other._issuerBindingContext != null)
                _issuerBindingContext = other._issuerBindingContext.Clone();
        }

        public SecureConversationSecurityTokenParameters()
        {
            // empty
        }

        public SecureConversationSecurityTokenParameters(SecurityBindingElement bootstrapSecurityBindingElement)
        {
            _bootstrapSecurityBindingElement = bootstrapSecurityBindingElement;
        }

        internal protected override bool HasAsymmetricKey { get { return false; } }

        public SecurityBindingElement BootstrapSecurityBindingElement
        {
            get
            {
                return _bootstrapSecurityBindingElement;
            }
            set
            {
                _bootstrapSecurityBindingElement = value;
            }
        }

        internal BindingContext IssuerBindingContext
        {
            get
            {
                return _issuerBindingContext;
            }
            set
            {
                if (value != null)
                {
                    value = value.Clone();
                }
                _issuerBindingContext = value;
            }
        }

        private ISecurityCapabilities BootstrapSecurityCapabilities
        {
            get
            {
                return _bootstrapSecurityBindingElement.GetIndividualProperty<ISecurityCapabilities>();
            }
        }

        internal protected override bool SupportsClientAuthentication
        {
            get
            {
                return this.BootstrapSecurityCapabilities == null ? false : this.BootstrapSecurityCapabilities.SupportsClientAuthentication;
            }
        }

        internal protected override bool SupportsServerAuthentication
        {
            get
            {
                return this.BootstrapSecurityCapabilities == null ? false : this.BootstrapSecurityCapabilities.SupportsServerAuthentication;
            }
        }

        internal protected override bool SupportsClientWindowsIdentity
        {
            get
            {
                return this.BootstrapSecurityCapabilities == null ? false : this.BootstrapSecurityCapabilities.SupportsClientWindowsIdentity;
            }
        }

        protected override SecurityTokenParameters CloneCore()
        {
            return new SecureConversationSecurityTokenParameters(this);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());

            sb.AppendLine(String.Format(CultureInfo.InvariantCulture, "RequireCancellation: {0}", _requireCancellation.ToString()));
            if (_bootstrapSecurityBindingElement == null)
            {
                sb.AppendLine(String.Format(CultureInfo.InvariantCulture, "BootstrapSecurityBindingElement: null"));
            }
            else
            {
                sb.AppendLine(String.Format(CultureInfo.InvariantCulture, "BootstrapSecurityBindingElement:"));
                sb.AppendLine("  " + this.BootstrapSecurityBindingElement.ToString().Trim().Replace("\n", "\n  "));
            }

            return sb.ToString().Trim();
        }
    }
}
