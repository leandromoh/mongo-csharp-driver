﻿/* Copyright 2017-present MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Driver
{
    /// <summary>
    /// An output document from a $changeStream pipeline stage.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    [BsonSerializer(typeof(ChangeStreamDocumentSerializer<>))]
    public sealed class ChangeStreamDocument<TDocument> : BsonDocumentBackedClass
    {
        // constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeStreamDocument{TDocument}"/> class.
        /// </summary>
        /// <param name="backingDocument">The backing document.</param>
        /// <param name="documentSerializer">The document serializer.</param>
        public ChangeStreamDocument(
            BsonDocument backingDocument,
            IBsonSerializer<TDocument> documentSerializer)
            : base(backingDocument, new ChangeStreamDocumentSerializer<TDocument>(documentSerializer))
        {
        }

        // public properties
        /// <summary>
        /// Gets the backing document.
        /// </summary>
        new public BsonDocument BackingDocument => base.BackingDocument;

        /// <summary>
        /// Gets the cluster time.
        /// </summary>
        /// <value>
        /// The cluster time.
        /// </value>
        public BsonTimestamp ClusterTime => GetValue<BsonTimestamp>(nameof(ClusterTime), null);

        /// <summary>
        /// Gets the namespace of the collection.
        /// </summary>
        /// <value>
        /// The namespace of the collection.
        /// </value>
        public CollectionNamespace CollectionNamespace => GetValue<CollectionNamespace>(nameof(CollectionNamespace), null);

        /// <summary>
        /// Gets the database namespace.
        /// </summary>
        /// <value>
        /// The database namespace.
        /// </value>
        public DatabaseNamespace DatabaseNamespace => GetValue<DatabaseNamespace>(nameof(DatabaseNamespace), null);

        /// <summary>
        /// Gets the document key.
        /// </summary>
        /// <value>
        /// The document key.
        /// </value>
        public BsonDocument DocumentKey => GetValue<BsonDocument>(nameof(DocumentKey), null);

        /// <summary>
        /// Gets the full document.
        /// </summary>
        /// <value>
        /// The full document.
        /// </value>
        public TDocument FullDocument
        {
            get
            {
                // if TDocument is BsonDocument avoid deserializing it again to prevent possible duplicate element name errors
                if (typeof(TDocument) == typeof(BsonDocument) && BackingDocument.TryGetValue("fullDocument", out var fullDocument))
                {
                    return (TDocument)(object)fullDocument.AsBsonDocument;
                }
                else
                {
                    return GetValue<TDocument>(nameof(FullDocument), default(TDocument));
                }
            }
        }

        /// <summary>
        /// Gets the full document before change.
        /// </summary>
        /// <value>
        /// The full document before change.
        /// </value>
        public TDocument FullDocumentBeforeChange
        {
            get
            {
                // if TDocument is BsonDocument avoid deserializing it again to prevent possible duplicate element name errors
                if (typeof(TDocument) == typeof(BsonDocument) && BackingDocument.TryGetValue("fullDocumentBeforeChange", out var fullDocumentBeforeChange))
                {
                    return (TDocument)(object)fullDocumentBeforeChange.AsBsonDocument;
                }
                else
                {
                    return GetValue<TDocument>(nameof(FullDocumentBeforeChange), default(TDocument));
                }
            }
        }

        /// <summary>
        /// Gets the type of the operation.
        /// </summary>
        /// <value>
        /// The type of the operation.
        /// </value>
        public ChangeStreamOperationType OperationType => GetValue<ChangeStreamOperationType>(nameof(OperationType), (ChangeStreamOperationType)(-1));

        /// <summary>
        /// Gets the new namespace for the ns collection. This field is omitted for all operation types except "rename".
        /// </summary>
        /// <value>
        /// The new namespace of the ns collection.
        /// </value>
        public CollectionNamespace RenameTo => GetValue<CollectionNamespace>(nameof(RenameTo), null);

        /// <summary>
        /// Gets the resume token.
        /// </summary>
        /// <value>
        /// The resume token.
        /// </value>
        public BsonDocument ResumeToken => GetValue<BsonDocument>(nameof(ResumeToken), null);

        /// <summary>
        /// Gets the update description.
        /// </summary>
        /// <value>
        /// The update description.
        /// </value>
        public ChangeStreamUpdateDescription UpdateDescription => GetValue<ChangeStreamUpdateDescription>(nameof(UpdateDescription), null);

        /// <summary>
        /// Gets the wall time of the change stream event.
        /// </summary>
        /// <value>
        /// The wall time.
        /// </value>
        public DateTime? WallTime => GetValue<DateTime?>(nameof(WallTime), null);
    }
}
