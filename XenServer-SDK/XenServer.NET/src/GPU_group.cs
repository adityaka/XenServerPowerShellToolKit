/*
 * Copyright (c) Citrix Systems, Inc.
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 
 *   1) Redistributions of source code must retain the above copyright
 *      notice, this list of conditions and the following disclaimer.
 * 
 *   2) Redistributions in binary form must reproduce the above
 *      copyright notice, this list of conditions and the following
 *      disclaimer in the documentation and/or other materials
 *      provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
 * FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
 * COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 */


using System;
using System.Collections;
using System.Collections.Generic;

using CookComputing.XmlRpc;


namespace XenAPI
{
    /// <summary>
    /// A group of compatible GPUs across the resource pool
    /// First published in XenServer 6.0.
    /// </summary>
    public partial class GPU_group : XenObject<GPU_group>
    {
        public GPU_group()
        {
        }

        public GPU_group(string uuid,
            string name_label,
            string name_description,
            List<XenRef<PGPU>> PGPUs,
            List<XenRef<VGPU>> VGPUs,
            string[] GPU_types,
            Dictionary<string, string> other_config,
            allocation_algorithm allocation_algorithm,
            List<XenRef<VGPU_type>> supported_VGPU_types,
            List<XenRef<VGPU_type>> enabled_VGPU_types)
        {
            this.uuid = uuid;
            this.name_label = name_label;
            this.name_description = name_description;
            this.PGPUs = PGPUs;
            this.VGPUs = VGPUs;
            this.GPU_types = GPU_types;
            this.other_config = other_config;
            this.allocation_algorithm = allocation_algorithm;
            this.supported_VGPU_types = supported_VGPU_types;
            this.enabled_VGPU_types = enabled_VGPU_types;
        }

        /// <summary>
        /// Creates a new GPU_group from a Proxy_GPU_group.
        /// </summary>
        /// <param name="proxy"></param>
        public GPU_group(Proxy_GPU_group proxy)
        {
            this.UpdateFromProxy(proxy);
        }

        public override void UpdateFrom(GPU_group update)
        {
            uuid = update.uuid;
            name_label = update.name_label;
            name_description = update.name_description;
            PGPUs = update.PGPUs;
            VGPUs = update.VGPUs;
            GPU_types = update.GPU_types;
            other_config = update.other_config;
            allocation_algorithm = update.allocation_algorithm;
            supported_VGPU_types = update.supported_VGPU_types;
            enabled_VGPU_types = update.enabled_VGPU_types;
        }

        internal void UpdateFromProxy(Proxy_GPU_group proxy)
        {
            uuid = proxy.uuid == null ? null : (string)proxy.uuid;
            name_label = proxy.name_label == null ? null : (string)proxy.name_label;
            name_description = proxy.name_description == null ? null : (string)proxy.name_description;
            PGPUs = proxy.PGPUs == null ? null : XenRef<PGPU>.Create(proxy.PGPUs);
            VGPUs = proxy.VGPUs == null ? null : XenRef<VGPU>.Create(proxy.VGPUs);
            GPU_types = proxy.GPU_types == null ? new string[] {} : (string [])proxy.GPU_types;
            other_config = proxy.other_config == null ? null : Maps.convert_from_proxy_string_string(proxy.other_config);
            allocation_algorithm = proxy.allocation_algorithm == null ? (allocation_algorithm) 0 : (allocation_algorithm)Helper.EnumParseDefault(typeof(allocation_algorithm), (string)proxy.allocation_algorithm);
            supported_VGPU_types = proxy.supported_VGPU_types == null ? null : XenRef<VGPU_type>.Create(proxy.supported_VGPU_types);
            enabled_VGPU_types = proxy.enabled_VGPU_types == null ? null : XenRef<VGPU_type>.Create(proxy.enabled_VGPU_types);
        }

        public Proxy_GPU_group ToProxy()
        {
            Proxy_GPU_group result_ = new Proxy_GPU_group();
            result_.uuid = (uuid != null) ? uuid : "";
            result_.name_label = (name_label != null) ? name_label : "";
            result_.name_description = (name_description != null) ? name_description : "";
            result_.PGPUs = (PGPUs != null) ? Helper.RefListToStringArray(PGPUs) : new string[] {};
            result_.VGPUs = (VGPUs != null) ? Helper.RefListToStringArray(VGPUs) : new string[] {};
            result_.GPU_types = GPU_types;
            result_.other_config = Maps.convert_to_proxy_string_string(other_config);
            result_.allocation_algorithm = allocation_algorithm_helper.ToString(allocation_algorithm);
            result_.supported_VGPU_types = (supported_VGPU_types != null) ? Helper.RefListToStringArray(supported_VGPU_types) : new string[] {};
            result_.enabled_VGPU_types = (enabled_VGPU_types != null) ? Helper.RefListToStringArray(enabled_VGPU_types) : new string[] {};
            return result_;
        }

        /// <summary>
        /// Creates a new GPU_group from a Hashtable.
        /// </summary>
        /// <param name="table"></param>
        public GPU_group(Hashtable table)
        {
            uuid = Marshalling.ParseString(table, "uuid");
            name_label = Marshalling.ParseString(table, "name_label");
            name_description = Marshalling.ParseString(table, "name_description");
            PGPUs = Marshalling.ParseSetRef<PGPU>(table, "PGPUs");
            VGPUs = Marshalling.ParseSetRef<VGPU>(table, "VGPUs");
            GPU_types = Marshalling.ParseStringArray(table, "GPU_types");
            other_config = Maps.convert_from_proxy_string_string(Marshalling.ParseHashTable(table, "other_config"));
            allocation_algorithm = (allocation_algorithm)Helper.EnumParseDefault(typeof(allocation_algorithm), Marshalling.ParseString(table, "allocation_algorithm"));
            supported_VGPU_types = Marshalling.ParseSetRef<VGPU_type>(table, "supported_VGPU_types");
            enabled_VGPU_types = Marshalling.ParseSetRef<VGPU_type>(table, "enabled_VGPU_types");
        }

        public bool DeepEquals(GPU_group other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return Helper.AreEqual2(this._uuid, other._uuid) &&
                Helper.AreEqual2(this._name_label, other._name_label) &&
                Helper.AreEqual2(this._name_description, other._name_description) &&
                Helper.AreEqual2(this._PGPUs, other._PGPUs) &&
                Helper.AreEqual2(this._VGPUs, other._VGPUs) &&
                Helper.AreEqual2(this._GPU_types, other._GPU_types) &&
                Helper.AreEqual2(this._other_config, other._other_config) &&
                Helper.AreEqual2(this._allocation_algorithm, other._allocation_algorithm) &&
                Helper.AreEqual2(this._supported_VGPU_types, other._supported_VGPU_types) &&
                Helper.AreEqual2(this._enabled_VGPU_types, other._enabled_VGPU_types);
        }

        public override string SaveChanges(Session session, string opaqueRef, GPU_group server)
        {
            if (opaqueRef == null)
            {
                System.Diagnostics.Debug.Assert(false, "Cannot create instances of this type on the server");
                return "";
            }
            else
            {
                if (!Helper.AreEqual2(_name_label, server._name_label))
                {
                    GPU_group.set_name_label(session, opaqueRef, _name_label);
                }
                if (!Helper.AreEqual2(_name_description, server._name_description))
                {
                    GPU_group.set_name_description(session, opaqueRef, _name_description);
                }
                if (!Helper.AreEqual2(_other_config, server._other_config))
                {
                    GPU_group.set_other_config(session, opaqueRef, _other_config);
                }
                if (!Helper.AreEqual2(_allocation_algorithm, server._allocation_algorithm))
                {
                    GPU_group.set_allocation_algorithm(session, opaqueRef, _allocation_algorithm);
                }

                return null;
            }
        }
        /// <summary>
        /// Get a record containing the current state of the given GPU_group.
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        public static GPU_group get_record(Session session, string _gpu_group)
        {
            return new GPU_group((Proxy_GPU_group)session.proxy.gpu_group_get_record(session.uuid, (_gpu_group != null) ? _gpu_group : "").parse());
        }

        /// <summary>
        /// Get a reference to the GPU_group instance with the specified UUID.
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_uuid">UUID of object to return</param>
        public static XenRef<GPU_group> get_by_uuid(Session session, string _uuid)
        {
            return XenRef<GPU_group>.Create(session.proxy.gpu_group_get_by_uuid(session.uuid, (_uuid != null) ? _uuid : "").parse());
        }

        /// <summary>
        /// Get all the GPU_group instances with the given label.
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_label">label of object to return</param>
        public static List<XenRef<GPU_group>> get_by_name_label(Session session, string _label)
        {
            return XenRef<GPU_group>.Create(session.proxy.gpu_group_get_by_name_label(session.uuid, (_label != null) ? _label : "").parse());
        }

        /// <summary>
        /// Get the uuid field of the given GPU_group.
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        public static string get_uuid(Session session, string _gpu_group)
        {
            return (string)session.proxy.gpu_group_get_uuid(session.uuid, (_gpu_group != null) ? _gpu_group : "").parse();
        }

        /// <summary>
        /// Get the name/label field of the given GPU_group.
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        public static string get_name_label(Session session, string _gpu_group)
        {
            return (string)session.proxy.gpu_group_get_name_label(session.uuid, (_gpu_group != null) ? _gpu_group : "").parse();
        }

        /// <summary>
        /// Get the name/description field of the given GPU_group.
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        public static string get_name_description(Session session, string _gpu_group)
        {
            return (string)session.proxy.gpu_group_get_name_description(session.uuid, (_gpu_group != null) ? _gpu_group : "").parse();
        }

        /// <summary>
        /// Get the PGPUs field of the given GPU_group.
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        public static List<XenRef<PGPU>> get_PGPUs(Session session, string _gpu_group)
        {
            return XenRef<PGPU>.Create(session.proxy.gpu_group_get_pgpus(session.uuid, (_gpu_group != null) ? _gpu_group : "").parse());
        }

        /// <summary>
        /// Get the VGPUs field of the given GPU_group.
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        public static List<XenRef<VGPU>> get_VGPUs(Session session, string _gpu_group)
        {
            return XenRef<VGPU>.Create(session.proxy.gpu_group_get_vgpus(session.uuid, (_gpu_group != null) ? _gpu_group : "").parse());
        }

        /// <summary>
        /// Get the GPU_types field of the given GPU_group.
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        public static string[] get_GPU_types(Session session, string _gpu_group)
        {
            return (string [])session.proxy.gpu_group_get_gpu_types(session.uuid, (_gpu_group != null) ? _gpu_group : "").parse();
        }

        /// <summary>
        /// Get the other_config field of the given GPU_group.
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        public static Dictionary<string, string> get_other_config(Session session, string _gpu_group)
        {
            return Maps.convert_from_proxy_string_string(session.proxy.gpu_group_get_other_config(session.uuid, (_gpu_group != null) ? _gpu_group : "").parse());
        }

        /// <summary>
        /// Get the allocation_algorithm field of the given GPU_group.
        /// First published in XenServer 6.2 SP1 Tech-Preview.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        public static allocation_algorithm get_allocation_algorithm(Session session, string _gpu_group)
        {
            return (allocation_algorithm)Helper.EnumParseDefault(typeof(allocation_algorithm), (string)session.proxy.gpu_group_get_allocation_algorithm(session.uuid, (_gpu_group != null) ? _gpu_group : "").parse());
        }

        /// <summary>
        /// Get the supported_VGPU_types field of the given GPU_group.
        /// First published in XenServer 6.2 SP1.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        public static List<XenRef<VGPU_type>> get_supported_VGPU_types(Session session, string _gpu_group)
        {
            return XenRef<VGPU_type>.Create(session.proxy.gpu_group_get_supported_vgpu_types(session.uuid, (_gpu_group != null) ? _gpu_group : "").parse());
        }

        /// <summary>
        /// Get the enabled_VGPU_types field of the given GPU_group.
        /// First published in XenServer 6.2 SP1.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        public static List<XenRef<VGPU_type>> get_enabled_VGPU_types(Session session, string _gpu_group)
        {
            return XenRef<VGPU_type>.Create(session.proxy.gpu_group_get_enabled_vgpu_types(session.uuid, (_gpu_group != null) ? _gpu_group : "").parse());
        }

        /// <summary>
        /// Set the name/label field of the given GPU_group.
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        /// <param name="_label">New value to set</param>
        public static void set_name_label(Session session, string _gpu_group, string _label)
        {
            session.proxy.gpu_group_set_name_label(session.uuid, (_gpu_group != null) ? _gpu_group : "", (_label != null) ? _label : "").parse();
        }

        /// <summary>
        /// Set the name/description field of the given GPU_group.
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        /// <param name="_description">New value to set</param>
        public static void set_name_description(Session session, string _gpu_group, string _description)
        {
            session.proxy.gpu_group_set_name_description(session.uuid, (_gpu_group != null) ? _gpu_group : "", (_description != null) ? _description : "").parse();
        }

        /// <summary>
        /// Set the other_config field of the given GPU_group.
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        /// <param name="_other_config">New value to set</param>
        public static void set_other_config(Session session, string _gpu_group, Dictionary<string, string> _other_config)
        {
            session.proxy.gpu_group_set_other_config(session.uuid, (_gpu_group != null) ? _gpu_group : "", Maps.convert_to_proxy_string_string(_other_config)).parse();
        }

        /// <summary>
        /// Add the given key-value pair to the other_config field of the given GPU_group.
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        /// <param name="_key">Key to add</param>
        /// <param name="_value">Value to add</param>
        public static void add_to_other_config(Session session, string _gpu_group, string _key, string _value)
        {
            session.proxy.gpu_group_add_to_other_config(session.uuid, (_gpu_group != null) ? _gpu_group : "", (_key != null) ? _key : "", (_value != null) ? _value : "").parse();
        }

        /// <summary>
        /// Remove the given key and its corresponding value from the other_config field of the given GPU_group.  If the key is not in that Map, then do nothing.
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        /// <param name="_key">Key to remove</param>
        public static void remove_from_other_config(Session session, string _gpu_group, string _key)
        {
            session.proxy.gpu_group_remove_from_other_config(session.uuid, (_gpu_group != null) ? _gpu_group : "", (_key != null) ? _key : "").parse();
        }

        /// <summary>
        /// Set the allocation_algorithm field of the given GPU_group.
        /// First published in XenServer 6.2 SP1 Tech-Preview.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        /// <param name="_allocation_algorithm">New value to set</param>
        public static void set_allocation_algorithm(Session session, string _gpu_group, allocation_algorithm _allocation_algorithm)
        {
            session.proxy.gpu_group_set_allocation_algorithm(session.uuid, (_gpu_group != null) ? _gpu_group : "", allocation_algorithm_helper.ToString(_allocation_algorithm)).parse();
        }

        /// <summary>
        /// 
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_name_label"></param>
        /// <param name="_name_description"></param>
        /// <param name="_other_config"></param>
        public static XenRef<GPU_group> create(Session session, string _name_label, string _name_description, Dictionary<string, string> _other_config)
        {
            return XenRef<GPU_group>.Create(session.proxy.gpu_group_create(session.uuid, (_name_label != null) ? _name_label : "", (_name_description != null) ? _name_description : "", Maps.convert_to_proxy_string_string(_other_config)).parse());
        }

        /// <summary>
        /// 
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_name_label"></param>
        /// <param name="_name_description"></param>
        /// <param name="_other_config"></param>
        public static XenRef<Task> async_create(Session session, string _name_label, string _name_description, Dictionary<string, string> _other_config)
        {
            return XenRef<Task>.Create(session.proxy.async_gpu_group_create(session.uuid, (_name_label != null) ? _name_label : "", (_name_description != null) ? _name_description : "", Maps.convert_to_proxy_string_string(_other_config)).parse());
        }

        /// <summary>
        /// 
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        public static void destroy(Session session, string _gpu_group)
        {
            session.proxy.gpu_group_destroy(session.uuid, (_gpu_group != null) ? _gpu_group : "").parse();
        }

        /// <summary>
        /// 
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        public static XenRef<Task> async_destroy(Session session, string _gpu_group)
        {
            return XenRef<Task>.Create(session.proxy.async_gpu_group_destroy(session.uuid, (_gpu_group != null) ? _gpu_group : "").parse());
        }

        /// <summary>
        /// 
        /// First published in XenServer 6.2 SP1 Tech-Preview.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        /// <param name="_vgpu_type">The VGPU_type for which the remaining capacity will be calculated</param>
        public static long get_remaining_capacity(Session session, string _gpu_group, string _vgpu_type)
        {
            return long.Parse((string)session.proxy.gpu_group_get_remaining_capacity(session.uuid, (_gpu_group != null) ? _gpu_group : "", (_vgpu_type != null) ? _vgpu_type : "").parse());
        }

        /// <summary>
        /// 
        /// First published in XenServer 6.2 SP1 Tech-Preview.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="_gpu_group">The opaque_ref of the given gpu_group</param>
        /// <param name="_vgpu_type">The VGPU_type for which the remaining capacity will be calculated</param>
        public static XenRef<Task> async_get_remaining_capacity(Session session, string _gpu_group, string _vgpu_type)
        {
            return XenRef<Task>.Create(session.proxy.async_gpu_group_get_remaining_capacity(session.uuid, (_gpu_group != null) ? _gpu_group : "", (_vgpu_type != null) ? _vgpu_type : "").parse());
        }

        /// <summary>
        /// Return a list of all the GPU_groups known to the system.
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        public static List<XenRef<GPU_group>> get_all(Session session)
        {
            return XenRef<GPU_group>.Create(session.proxy.gpu_group_get_all(session.uuid).parse());
        }

        /// <summary>
        /// Get all the GPU_group Records at once, in a single XML RPC call
        /// First published in XenServer 6.0.
        /// </summary>
        /// <param name="session">The session</param>
        public static Dictionary<XenRef<GPU_group>, GPU_group> get_all_records(Session session)
        {
            return XenRef<GPU_group>.Create<Proxy_GPU_group>(session.proxy.gpu_group_get_all_records(session.uuid).parse());
        }

        /// <summary>
        /// Unique identifier/object reference
        /// </summary>
        public virtual string uuid
        {
            get { return _uuid; }
            set
            {
                if (!Helper.AreEqual(value, _uuid))
                {
                    _uuid = value;
                    Changed = true;
                    NotifyPropertyChanged("uuid");
                }
            }
        }
        private string _uuid;

        /// <summary>
        /// a human-readable name
        /// </summary>
        public virtual string name_label
        {
            get { return _name_label; }
            set
            {
                if (!Helper.AreEqual(value, _name_label))
                {
                    _name_label = value;
                    Changed = true;
                    NotifyPropertyChanged("name_label");
                }
            }
        }
        private string _name_label;

        /// <summary>
        /// a notes field containing human-readable description
        /// </summary>
        public virtual string name_description
        {
            get { return _name_description; }
            set
            {
                if (!Helper.AreEqual(value, _name_description))
                {
                    _name_description = value;
                    Changed = true;
                    NotifyPropertyChanged("name_description");
                }
            }
        }
        private string _name_description;

        /// <summary>
        /// List of pGPUs in the group
        /// </summary>
        public virtual List<XenRef<PGPU>> PGPUs
        {
            get { return _PGPUs; }
            set
            {
                if (!Helper.AreEqual(value, _PGPUs))
                {
                    _PGPUs = value;
                    Changed = true;
                    NotifyPropertyChanged("PGPUs");
                }
            }
        }
        private List<XenRef<PGPU>> _PGPUs;

        /// <summary>
        /// List of vGPUs using the group
        /// </summary>
        public virtual List<XenRef<VGPU>> VGPUs
        {
            get { return _VGPUs; }
            set
            {
                if (!Helper.AreEqual(value, _VGPUs))
                {
                    _VGPUs = value;
                    Changed = true;
                    NotifyPropertyChanged("VGPUs");
                }
            }
        }
        private List<XenRef<VGPU>> _VGPUs;

        /// <summary>
        /// List of GPU types (vendor+device ID) that can be in this group
        /// </summary>
        public virtual string[] GPU_types
        {
            get { return _GPU_types; }
            set
            {
                if (!Helper.AreEqual(value, _GPU_types))
                {
                    _GPU_types = value;
                    Changed = true;
                    NotifyPropertyChanged("GPU_types");
                }
            }
        }
        private string[] _GPU_types;

        /// <summary>
        /// Additional configuration
        /// </summary>
        public virtual Dictionary<string, string> other_config
        {
            get { return _other_config; }
            set
            {
                if (!Helper.AreEqual(value, _other_config))
                {
                    _other_config = value;
                    Changed = true;
                    NotifyPropertyChanged("other_config");
                }
            }
        }
        private Dictionary<string, string> _other_config;

        /// <summary>
        /// Current allocation of vGPUs to pGPUs for this group
        /// First published in XenServer 6.2 SP1 Tech-Preview.
        /// </summary>
        public virtual allocation_algorithm allocation_algorithm
        {
            get { return _allocation_algorithm; }
            set
            {
                if (!Helper.AreEqual(value, _allocation_algorithm))
                {
                    _allocation_algorithm = value;
                    Changed = true;
                    NotifyPropertyChanged("allocation_algorithm");
                }
            }
        }
        private allocation_algorithm _allocation_algorithm;

        /// <summary>
        /// vGPU types supported on at least one of the pGPUs in this group
        /// First published in XenServer 6.2 SP1.
        /// </summary>
        public virtual List<XenRef<VGPU_type>> supported_VGPU_types
        {
            get { return _supported_VGPU_types; }
            set
            {
                if (!Helper.AreEqual(value, _supported_VGPU_types))
                {
                    _supported_VGPU_types = value;
                    Changed = true;
                    NotifyPropertyChanged("supported_VGPU_types");
                }
            }
        }
        private List<XenRef<VGPU_type>> _supported_VGPU_types;

        /// <summary>
        /// vGPU types supported on at least one of the pGPUs in this group
        /// First published in XenServer 6.2 SP1.
        /// </summary>
        public virtual List<XenRef<VGPU_type>> enabled_VGPU_types
        {
            get { return _enabled_VGPU_types; }
            set
            {
                if (!Helper.AreEqual(value, _enabled_VGPU_types))
                {
                    _enabled_VGPU_types = value;
                    Changed = true;
                    NotifyPropertyChanged("enabled_VGPU_types");
                }
            }
        }
        private List<XenRef<VGPU_type>> _enabled_VGPU_types;
    }
}