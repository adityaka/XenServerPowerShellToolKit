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
using System.Management.Automation;
using System.Text;

using XenAPI;

namespace Citrix.XenServer.Commands
{
    [Cmdlet(VerbsCommon.Get, "XenNetworkProperty", SupportsShouldProcess = false)]
    public class GetXenNetworkProperty : XenServerCmdlet
    {
        #region Cmdlet Parameters

        [Parameter(ParameterSetName = "XenObject", Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public XenAPI.Network Network { get; set; }
        
        [Parameter(ParameterSetName = "Ref", Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        [Alias("opaque_ref")]
        public XenRef<XenAPI.Network> Ref { get; set; }


        [Parameter(Mandatory = true)]
        public XenNetworkProperty XenProperty { get; set; }
        
        #endregion

        #region Cmdlet Methods
        
        protected override void ProcessRecord()
        {
            GetSession();
            
            string network = ParseNetwork();
            
            switch (XenProperty)
            {
                case XenNetworkProperty.Uuid:
                    ProcessRecordUuid(network);
                    break;
                case XenNetworkProperty.NameLabel:
                    ProcessRecordNameLabel(network);
                    break;
                case XenNetworkProperty.NameDescription:
                    ProcessRecordNameDescription(network);
                    break;
                case XenNetworkProperty.AllowedOperations:
                    ProcessRecordAllowedOperations(network);
                    break;
                case XenNetworkProperty.CurrentOperations:
                    ProcessRecordCurrentOperations(network);
                    break;
                case XenNetworkProperty.VIFs:
                    ProcessRecordVIFs(network);
                    break;
                case XenNetworkProperty.PIFs:
                    ProcessRecordPIFs(network);
                    break;
                case XenNetworkProperty.MTU:
                    ProcessRecordMTU(network);
                    break;
                case XenNetworkProperty.OtherConfig:
                    ProcessRecordOtherConfig(network);
                    break;
                case XenNetworkProperty.Bridge:
                    ProcessRecordBridge(network);
                    break;
                case XenNetworkProperty.Blobs:
                    ProcessRecordBlobs(network);
                    break;
                case XenNetworkProperty.Tags:
                    ProcessRecordTags(network);
                    break;
                case XenNetworkProperty.DefaultLockingMode:
                    ProcessRecordDefaultLockingMode(network);
                    break;
                case XenNetworkProperty.AssignedIps:
                    ProcessRecordAssignedIps(network);
                    break;
            }
            
            UpdateSessions();
        }
        
        #endregion
    
        #region Private Methods

        private string ParseNetwork()
        {
            string network = null;

            if (Network != null)
                network = (new XenRef<XenAPI.Network>(Network)).opaque_ref;
            else if (Ref != null)
                network = Ref.opaque_ref;
            else
            {
                ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("At least one of the parameters 'Network', 'Ref', 'Uuid' must be set"),
                    string.Empty,
                    ErrorCategory.InvalidArgument,
                    Network));
            }

            return network;
        }

        private void ProcessRecordUuid(string network)
        {
            RunApiCall(()=>
            {
                    string obj = XenAPI.Network.get_uuid(session, network);

                        WriteObject(obj, true);
            });
        }

        private void ProcessRecordNameLabel(string network)
        {
            RunApiCall(()=>
            {
                    string obj = XenAPI.Network.get_name_label(session, network);

                        WriteObject(obj, true);
            });
        }

        private void ProcessRecordNameDescription(string network)
        {
            RunApiCall(()=>
            {
                    string obj = XenAPI.Network.get_name_description(session, network);

                        WriteObject(obj, true);
            });
        }

        private void ProcessRecordAllowedOperations(string network)
        {
            RunApiCall(()=>
            {
                    List<network_operations> obj = XenAPI.Network.get_allowed_operations(session, network);

                        WriteObject(obj, true);
            });
        }

        private void ProcessRecordCurrentOperations(string network)
        {
            RunApiCall(()=>
            {
                    var dict = XenAPI.Network.get_current_operations(session, network);

                        Hashtable ht = CommonCmdletFunctions.ConvertDictionaryToHashtable(dict);
                        WriteObject(ht, true);
            });
        }

        private void ProcessRecordVIFs(string network)
        {
            RunApiCall(()=>
            {
                    var refs = XenAPI.Network.get_VIFs(session, network);

                        var records = new List<XenAPI.VIF>();

                        foreach (var _ref in refs)
                        {
                            if (_ref.opaque_ref == "OpaqueRef:NULL")
                                continue;
                        
                            var record = XenAPI.VIF.get_record(session, _ref);
                            record.opaque_ref = _ref.opaque_ref;
                            records.Add(record);
                        }

                        WriteObject(records, true);
            });
        }

        private void ProcessRecordPIFs(string network)
        {
            RunApiCall(()=>
            {
                    var refs = XenAPI.Network.get_PIFs(session, network);

                        var records = new List<XenAPI.PIF>();

                        foreach (var _ref in refs)
                        {
                            if (_ref.opaque_ref == "OpaqueRef:NULL")
                                continue;
                        
                            var record = XenAPI.PIF.get_record(session, _ref);
                            record.opaque_ref = _ref.opaque_ref;
                            records.Add(record);
                        }

                        WriteObject(records, true);
            });
        }

        private void ProcessRecordMTU(string network)
        {
            RunApiCall(()=>
            {
                    long obj = XenAPI.Network.get_MTU(session, network);

                        WriteObject(obj, true);
            });
        }

        private void ProcessRecordOtherConfig(string network)
        {
            RunApiCall(()=>
            {
                    var dict = XenAPI.Network.get_other_config(session, network);

                        Hashtable ht = CommonCmdletFunctions.ConvertDictionaryToHashtable(dict);
                        WriteObject(ht, true);
            });
        }

        private void ProcessRecordBridge(string network)
        {
            RunApiCall(()=>
            {
                    string obj = XenAPI.Network.get_bridge(session, network);

                        WriteObject(obj, true);
            });
        }

        private void ProcessRecordBlobs(string network)
        {
            RunApiCall(()=>
            {
                    var dict = XenAPI.Network.get_blobs(session, network);

                        Hashtable ht = CommonCmdletFunctions.ConvertDictionaryToHashtable(dict);
                        WriteObject(ht, true);
            });
        }

        private void ProcessRecordTags(string network)
        {
            RunApiCall(()=>
            {
                    string[] obj = XenAPI.Network.get_tags(session, network);

                        WriteObject(obj, true);
            });
        }

        private void ProcessRecordDefaultLockingMode(string network)
        {
            RunApiCall(()=>
            {
                    network_default_locking_mode obj = XenAPI.Network.get_default_locking_mode(session, network);

                        WriteObject(obj, true);
            });
        }

        private void ProcessRecordAssignedIps(string network)
        {
            RunApiCall(()=>
            {
                    var dict = XenAPI.Network.get_assigned_ips(session, network);

                        Hashtable ht = CommonCmdletFunctions.ConvertDictionaryToHashtable(dict);
                        WriteObject(ht, true);
            });
        }

        #endregion
    }
    
    public enum XenNetworkProperty
    {
        Uuid,
        NameLabel,
        NameDescription,
        AllowedOperations,
        CurrentOperations,
        VIFs,
        PIFs,
        MTU,
        OtherConfig,
        Bridge,
        Blobs,
        Tags,
        DefaultLockingMode,
        AssignedIps
    }

}
